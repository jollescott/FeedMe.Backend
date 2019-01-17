using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ramsey.NET.Areas.Identity.Data;
using Ramsey.NET.Models;

[assembly: HostingStartup(typeof(Ramsey.NET.Areas.Identity.IdentityHostingStartup))]
namespace Ramsey.NET.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AdminContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AdminContextConnection")));

                services.AddDefaultIdentity<RamseyAdmin>()
                    .AddEntityFrameworkStores<AdminContext>();
            });
        }
    }
}