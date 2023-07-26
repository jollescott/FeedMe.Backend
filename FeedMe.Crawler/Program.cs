using FeedMe.Crawler.Interfaces;
using FeedMe.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ramsey.Core.Implementations;
using Ramsey.Core.Interfaces;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Enums;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ICrawlerService, CrawlerService>();
builder.Services.AddScoped<IWordRemover, IllegalRemover>();
builder.Services.AddScoped<IRecipeManager, SqlRecipeManager>();
builder.Services.AddDbContext<IRamseyContext, RamseyContext>(options => options.UseSqlite($"Data Source=ramsey.db"));

using IHost host = builder.Build();

var crawler = host.Services.GetRequiredService<ICrawlerService>();

foreach (RecipeProvider provider in Enum.GetValues(typeof(RecipeProvider)))
{
    await crawler.ReindexProviderAsync(provider);
}

await host.RunAsync();