using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactViewEngine;
using System.IO;

namespace Pantry
{
    public class Startup
    {
        private string _contentRoot;
        private bool _isDevelopment = false;

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            _contentRoot = env.ContentRootPath;
            _isDevelopment = env.IsDevelopment();

            var builder = new ConfigurationBuilder()
                .SetBasePath(_contentRoot)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                // if Mac
                .AddJsonFile("appsettings.osx.json")
                .AddEnvironmentVariables();

            if (env.IsDevelopment()) {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddReactViewEngine(new NodeServerOptions
            {
                IsDevelopment = true, //_isDevelopment,
                PackageRoot = Path.Combine(_contentRoot, "app"),
                Port = 3000,
                PathToNpm = Configuration["PathToNpm"]
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
