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

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: Migrate endpoints.
            services.AddMvc().AddMvcOptions(opt => opt.EnableEndpointRouting = false);

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                var connectionString = Configuration.GetConnectionString("RAM_CONNECTION_STRING");
                services.AddDbContext<IRamseyContext, RamseyContext>(options => options.UseNpgsql(connectionString!));
            }
            else
            {
                services.AddDbContext<IRamseyContext, RamseyContext>(options =>
                    options.UseSqlite($"Data Source=ramsey.db"));
            }
            
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

            if (env.IsDevelopment()) return;
            RecurringJob.AddOrUpdate(() => serviceProvider.GetRequiredService<ICrawlerService>().StartIndexUpdate(), Cron.Weekly);
            BackgroundJob.Enqueue(() => serviceProvider.GetRequiredService<ICrawlerService>().StartIndexUpdate());
        }
    }
}
