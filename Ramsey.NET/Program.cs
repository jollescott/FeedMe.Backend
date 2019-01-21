using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ramsey.NET.Implementations;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;

namespace Ramsey.NET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<IRamseyContext>();
                context.Database.Migrate();

                var adminService = services.GetRequiredService<IAdminService>();

                AdminSeeder.Seed(context, adminService);
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json");
                config.AddCommandLine(args);
                config.AddEnvironmentVariables();
            })
            .UseStartup<Startup>();
    }

    internal class AdminSeeder
    {
        internal static void Seed(IRamseyContext context, IAdminService adminService)
        {
            if (context.AdminUsers.Any(x => x.Username == "root"))
                return;

            adminService.CreateAsync(new AdminUser
            {
                FirstName = "Joel",
                LastName = "Linder",
                Id = 0,
                Username = "root",
            }, "superkuk666").Wait();
        }
    }
}
