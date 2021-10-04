using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestiRecord.API.Data;
using TestiRecord.API.Services;

namespace TestiRecord.API
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

            services.AddCors();
            services.AddTransient<TestiService>();

            services.AddDbContext<DataContext>(x =>
            {
                x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSwaggerDocument();
            //services.AddControllersWithViews();
            var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

            var mvc =
                     services.AddMvc(
                     options =>
                     {
                         options.Filters.Add(new AuthorizeFilter(policy));
                         options.EnableEndpointRouting = false;
                         // options.Filters.Add(new AuthorizeFilter(policy)); //https://dzone.com/articles/global-authorization-filter-in-net-core-net-core-s
                         options.Filters.Add(new ResponseCacheAttribute() { NoStore = true, Location = ResponseCacheLocation.None });
                     }
                     ).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot/dist";
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseMvc(routes =>
            {

                //  routes.MapAreaRoute( "app-route","app", "app/{action=Report}/{folderName?}"); 
                ////   routes.MapRoute("app", "app/{action=Report}/{folderName?}");
                //routes.MapRoute(
                //     name: "app",
                //     template: "{controller=App}/{action=Index}/{id?}");

                routes.MapRoute(
                     name: "app",
                     template: "App/{action}/{folderName}"
                // defaults: new { controller = "App", action = "Report", folderName = "" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "dist");

                ////adding this route seems okay but messes with the defaultness of the dist route
                //routes.MapRoute(
                //     name: "app",
                //     template: "{controller=App}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.DefaultPage = "/index.html";
                spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                {
                    OnPrepareResponse = context =>
                    {
                        // never cache index.html
                        if (context.File.Name == "index.html")
                        {
                            context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                            context.Context.Response.Headers.Add("Expires", "-1");
                        }
                    }
                };
                // spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions() { a};
                // spa.Options.SourcePath = "ClientApp";

            });
        }
    }
}
