using Ramsey.NET.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Enums;
using Ramsey.NET.Auto;
using Ramsey.NET.Auto.Configs;
using Hangfire;

namespace Ramsey.NET.Implementations
{
    public class CrawlerService : ICrawlerService
    {
        private readonly IRamseyContext _context;
        private readonly IRecipeManager _recipeManager;

        private readonly Dictionary<RecipeProvider, IRecipeCrawler> Crawlers;

        public CrawlerService(IRamseyContext context, IRecipeManager recipeManager)
        {
            _context = context;
            _recipeManager = recipeManager;

            Crawlers = new Dictionary<RecipeProvider, IRecipeCrawler>
            {
                {RecipeProvider.ReceptSe, new RamseyAuto(new ReceptSeConfig(), context) },
                {RecipeProvider.Tasteline, new RamseyAuto(new TastelineConfig(), context) },
                {RecipeProvider.Hemmets, new RamseyAuto(new HemmetsConfig(), context) },
                {RecipeProvider.ICA, new RamseyAuto(new IcaConfig(), context) },
            };
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task ReindexProviderAsync(RecipeProvider provider)
        {
            var crawler = Crawlers[provider];
            await crawler.ScrapeRecipesAsync(_recipeManager);
        }

        public Task<RecipeDtoV2> ScrapeRecipeAsync(string url, RecipeProvider provider)
        {
            if(!Crawlers.ContainsKey(provider))
                throw new Exception("Crawler mapped to: " + Enum.GetName(typeof(RecipeProvider), provider) + " was not found.");
            
            var crawler = Crawlers[provider];
            return crawler.ScrapeRecipeAsync(url, true);
        }

        public void StartIndexUpdate()
        {
            string lastJobId = null;
            foreach(var provider in Crawlers.Keys)
            {
                if (lastJobId == null)
                    lastJobId = BackgroundJob.Enqueue(() => ReindexProviderAsync(provider));
                else
                    lastJobId = BackgroundJob.ContinueWith(lastJobId, () => ReindexProviderAsync(provider));
            }
        }
    }
}
