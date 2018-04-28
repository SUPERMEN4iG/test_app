using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using test_app.shared;
using test_app.shared.Data;
using test_app.shared.Repositories;
using test_app.shared.ViewModels;

namespace test_app.api_admin
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
            var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier; // Переопределяем UserIdClaimType на NameIdentifier важно для JWT!
                                                                                    //options.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(ITokenProviderDescriptor<ApplicationUser>)));
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            #region биндинг non-repository для сущностей
            services.AddScoped<IRepositoryFactory, UnitOfWork<ApplicationDbContext>>();
            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();
            services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();
            #endregion

            #region биндинг repository сущностей
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWinnerRepository, WinnerRepository>();
            services.AddScoped<ICaseRepository, CaseRepository>();
            services.AddScoped<IG2ARepository, G2ARepository>();
            services.AddScoped<IBotRepository, BotRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<ISkinRepository, SkinRepository>();
            services.AddScoped<ICaseDropRepository, CaseDropRepository>();
            #endregion

            //services.AddWebSocketManager();

            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .ConfigureApplicationPartManager(manager =>
                {
                    var oldMetadataReferenceFeatureProvider = manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider);
                    manager.FeatureProviders.Remove(oldMetadataReferenceFeatureProvider);
                    manager.FeatureProviders.Add(new ReferencesMetadataReferenceFeatureProvider());
                });
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors("CorsPolicy");

            app.UseWebSockets(new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(60),
                ReceiveBufferSize = 4 * 1024,
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
