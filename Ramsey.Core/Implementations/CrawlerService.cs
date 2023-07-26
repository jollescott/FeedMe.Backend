using FeedMe.Crawler.Interfaces;
using Ramsey.Core.Interfaces;
using Ramsey.NET.Auto.Configs;
using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramsey.Core.Implementations
{
    public class CrawlerService : ICrawlerService
    {
        private readonly IRecipeManager _recipeManager;

        private readonly Dictionary<RecipeProvider, IRecipeCrawler> _crawlers;

        public CrawlerService(IRecipeManager recipeManager, IWordRemover illegalRemover)
        {
            _recipeManager = recipeManager;

            _crawlers = new Dictionary<RecipeProvider, IRecipeCrawler>
            {
                {RecipeProvider.ReceptSe, new RamseyAuto(new ReceptSeConfig(), illegalRemover) },
                {RecipeProvider.Tasteline, new RamseyAuto(new TastelineConfig(), illegalRemover) },
                {RecipeProvider.Hemmets, new RamseyAuto(new HemmetsConfig(), illegalRemover) },
                {RecipeProvider.ICA, new RamseyAuto(new IcaConfig(), illegalRemover) },
            };
        }

        public async Task ReindexProviderAsync(RecipeProvider provider)
        {
            var crawler = _crawlers[provider];
            await crawler.ScrapeRecipesAsync(_recipeManager);
        }

        public Task<RecipeDtoV2> ScrapeRecipeAsync(string url, RecipeProvider provider)
        {
            if (!_crawlers.ContainsKey(provider))
                throw new Exception("Crawler mapped to: " + Enum.GetName(typeof(RecipeProvider), provider) + " was not found.");

            var crawler = _crawlers[provider];
            return crawler.ScrapeRecipeAsync(url, true);
        }

        public void StartIndexUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
