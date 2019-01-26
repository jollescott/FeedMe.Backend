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

namespace Ramsey.NET.Implementations
{
    public class CrawlerService : ICrawlerService
    {
        private readonly IRamseyContext _context;
        private readonly IRecipeManager _recipeManager;

        private readonly Dictionary<RecipeProvider, IRecipeCrawler> Crawlers = new Dictionary<RecipeProvider, IRecipeCrawler>
        {
            {RecipeProvider.Hemmets, new RamseyAuto(new HemmetsConfig()) },
            {RecipeProvider.ReceptSe, new RamseyAuto(new ReceptSeConfig()) },
            {RecipeProvider.Tasteline, new RamseyAuto(new TastelineConfig()) },
            {RecipeProvider.ICA, new RamseyAuto(new IcaConfig()) },
        };

        public CrawlerService(IRamseyContext context, IRecipeManager recipeManager)
        {
            _context = context;
            _recipeManager = recipeManager;
        }

        public async Task UpdateIndexAsync()
        {   
            foreach (var crawler in Crawlers)
            {
                await crawler.Value.ScrapeRecipesAsync(_recipeManager, -1);
            }
        }

        public Task<RecipeDtoV2> ScrapeRecipeAsync(string url, RecipeProvider provider)
        {
            if(!Crawlers.ContainsKey(provider))
                throw new Exception("Crawler mapped to: " + Enum.GetName(typeof(RecipeProvider), provider) + " was not found.");
            
            var crawler = Crawlers[provider];
            return crawler.ScrapeRecipeAsync(url, true);
        }
    }
}
