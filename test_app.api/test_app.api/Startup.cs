using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.AspNetCore.Cors.Infrastructure;

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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddOptions();
            services.Configure<SteamOptions>(options => { options.ApiKey = Configuration["ApiKey"]; });

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })

                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/signout";
                }).AddSteam(options =>
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors("CorsPolicy");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
