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
using Ramsey.Core;
using Ramsey.NET.Auto.Implementations;

namespace Ramsey.NET.Implementations
{
    public class CrawlerService : ICrawlerService
    {
        private readonly IRamseyContext _context;
        private readonly IRecipeManager _recipeManager;

        private readonly Dictionary<RecipeProvider, IRecipeCrawler> _crawlers;

        public CrawlerService(IRamseyContext context, IRecipeManager recipeManager, IWordRemover illegalRemover)
        {
            _context = context;
            _recipeManager = recipeManager;

            _crawlers = new Dictionary<RecipeProvider, IRecipeCrawler>
            {
                {RecipeProvider.ReceptSe, new RamseyAuto(new ReceptSeConfig(), illegalRemover) },
                {RecipeProvider.Tasteline, new RamseyAuto(new TastelineConfig(), illegalRemover) },
                {RecipeProvider.Hemmets, new RamseyAuto(new HemmetsConfig(), illegalRemover) },
                {RecipeProvider.ICA, new RamseyAuto(new IcaConfig(), illegalRemover) },
            };
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task ReindexProviderAsync(RecipeProvider provider)
        {
            var crawler = _crawlers[provider];
            await crawler.ScrapeRecipesAsync(_recipeManager);
        }

        public Task<RecipeDtoV2> ScrapeRecipeAsync(string url, RecipeProvider provider)
        {
            if(!_crawlers.ContainsKey(provider))
                throw new Exception("Crawler mapped to: " + Enum.GetName(typeof(RecipeProvider), provider) + " was not found.");
            
            var crawler = _crawlers[provider];
            return crawler.ScrapeRecipeAsync(url, true);
        }

        public void StartIndexUpdate()
        {
            string lastJobId = null;
            foreach(var provider in _crawlers.Keys)
            {
                lastJobId = lastJobId == null ? BackgroundJob.Enqueue(() => ReindexProviderAsync(provider)) : 
                    BackgroundJob.ContinueJobWith(lastJobId, () => ReindexProviderAsync(provider));
            }
        }
    }
}
