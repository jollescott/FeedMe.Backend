using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ramsey.NET.Implementations;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using System;
using System.Text;
using Hangfire.SQLite;
using Ramsey.NET.Extensions;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.NET.Ingredients.Implementations;
using Ramsey.NET.Ingredients.Interfaces;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });


            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                services.AddDbContext<IRamseyContext, RamseyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("RamseyRelease")));
                services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("RamseyRelease")));
            }
            else
            {
                services.AddDbContext<IRamseyContext, RamseyContext>(options => options.ConnectRamseyTestServer(Configuration));
                services.AddHangfire(config => config.ConnectHangfireTest(Configuration));
            }

            services.AddScoped<IRecipeManager, SqlRecipeManager>();
            services.AddScoped<IIngredientResolver, BasicIngredientResolver>();
            services.AddScoped<ICrawlerService, CrawlerService>();
            services.AddScoped<IPatcherService, IngredientPatcherService>();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(serviceProvider));

            app.UseHangfireServer();
        }
    }
}
