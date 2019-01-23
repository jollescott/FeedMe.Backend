using Ramsey.NET.Interfaces;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ramsey.NET.Crawlers.Implementations.Hemmets;
using Ramsey.NET.Crawlers.Implementations.Mathem;
using Ramsey.NET.Crawlers.Implementations.ReceptSe;
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
            /* {RecipeProvider.Hemmets, new HemmetsRecipeCrawler()},
             {RecipeProvider.ReceptSe, new ReceptSeCrawler()},
             {RecipeProvider.Mathem, new MathemCrawler()}*/
            {RecipeProvider.Hemmets, new RamseyAuto(new HemmetsConfig()) }
        };

        public CrawlerService(IRamseyContext context, IRecipeManager recipeManager)
        {
            _context = context;
            _recipeManager = recipeManager;
        }

        public async Task UpdateIndexAsync()
        {   
            foreach (var crawler in Crawlers.Values)
            {
                await crawler.ScrapeRecipesAsync(_recipeManager, -1);
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
