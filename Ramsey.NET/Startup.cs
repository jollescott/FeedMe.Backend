using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ramsey.NET.Implementations;
using Ramsey.NET.Interfaces;
using System;
using System.Text;
using Ramsey.NET.Extensions;
using Ramsey.NET.Shared.Interfaces;
using System.Threading.Tasks;
using Hangfire.MemoryStorage;
using System.Globalization;
using Ramsey.Core;
using Ramsey.NET.Auto.Implementations;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace Ramsey.NET
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            */

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });


            var connectionString = Configuration.GetConnectionString(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ? "RamseyRelease" : "RamseyDebug");
            services.AddDbContext<IRamseyContext, RamseyContext>(options => options.UseNpgsql(connectionString));

            services.AddHangfire(config => config.UseMemoryStorage(new MemoryStorageOptions { FetchNextJobTimeout = TimeSpan.FromHours(24), JobExpirationCheckInterval = TimeSpan.FromMinutes(120) }));

            services.AddScoped<IWordRemover, BasicWordRemover>();
            services.AddScoped<IRecipeManager, SqlRecipeManager>();
            services.AddScoped<ICrawlerService, CrawlerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            var cultureInfo = new CultureInfo("sv-SE");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(serviceProvider));

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            if (!env.IsDevelopment())
            {
                RecurringJob.AddOrUpdate(() => serviceProvider.GetRequiredService<ICrawlerService>().StartIndexUpdate(), Cron.Weekly);
                BackgroundJob.Enqueue(() => serviceProvider.GetRequiredService<ICrawlerService>().StartIndexUpdate());
            }

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
