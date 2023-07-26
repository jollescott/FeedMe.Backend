using FeedMe.Crawler.Implementations;
using FeedMe.Crawler.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ramsey.Shared.Enums;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ICrawlerService, CrawlerService>();

using IHost host = builder.Build();

var crawler = host.Services.GetRequiredService<ICrawlerService>();

foreach (RecipeProvider provider in Enum.GetValues(typeof(RecipeProvider)))
{
    await crawler.ReindexProviderAsync(provider);
}

await host.RunAsync();