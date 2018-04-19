using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using test_app.api.Data;
using test_app.api.Models;
using test_app.api.Services;
using AspNet.Security.OpenId;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using test_app.api.Models.Configuration;
using test_app.api.Logic.Extensions;
using test_app.api.Logic.LastWinnersSocket;

namespace test_app.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            // var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            //    services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier; // Переопределяем UserIdClaimType на NameIdentifier важно для JWT!
                    //options.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(ITokenProviderDescriptor<ApplicationUser>)));
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.ClaimsIdentity.UserNameClaimType = "";
            //    //options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            //});

            services.AddOptions();
            services.Configure<SteamOptions>(options => { options.ApiKey = Configuration["ApiKey"]; });

            services.Configure<ClientConfigurations>(
                Configuration.GetSection("Client"));

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            // TODO: Сделать response, когда JWT-токен умер
            services.AddAuthentication(options => {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateActor = true,
                        ValidateAudience = false,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                })
                .AddSteam(options =>
                {
                    options.ApplicationKey = Configuration["ApiKey"];
                    //options.CallbackPath = "/steam";
                    //options.Events.OnAuthenticated += (context) =>
                    //{
                    //    return new Task(() => { });
                    //};
                });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            services.AddMvc().ConfigureApplicationPartManager(manager =>
            {
                var oldMetadataReferenceFeatureProvider = manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider);
                manager.FeatureProviders.Remove(oldMetadataReferenceFeatureProvider);
                manager.FeatureProviders.Add(new ReferencesMetadataReferenceFeatureProvider());
            });
            services.AddMemoryCache();
      //      services.AddWebSocketManager();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();

                using (var serviceScope =
                    app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
                    serviceScope.ServiceProvider.GetService<ApplicationDbContext>().EnsureSeedData();
                }
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Диагностика выполнения для каждого request
            app.Use(async (context, next) =>
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                await next.Invoke();
                sw.Stop();

                System.Diagnostics.Debug.WriteLine(String.Format("[{0} ms] {1} params: [{2}]", 
                    sw.ElapsedMilliseconds, 
                    context.Request.Path.ToUriComponent(),
                    context.Request.Query.Aggregate(new StringBuilder(), (s, i) => s.Append(String.Format("{0}: {1},", i.Key.ToString(), i.Value.ToString()))).ToString()
                    ));
            });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors("CorsPolicy");

            app.UseWebSockets();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

           // app.MapWebSocketManager("/lastwinners", serviceProvider.GetService<LastWinnersHandler>());
        }
    }
}
