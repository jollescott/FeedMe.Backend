using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Crawlers.Implementations.Hemmets
{
    public abstract class AHemmetsRecipeCrawler : IRecipeCrawler
    {
        public abstract Task<int> GetRecipeCountAsync();

        public abstract Task<List<RecipeMetaDtoV2>> ScrapePagesAsync(int count, int offset);

        public abstract Task<List<RecipeMetaDtoV2>> ScrapePageAsync(int offset);
        public abstract Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false);

        public abstract int ScrapeRecipeCount(string html);
        public abstract IEnumerable<string> ScapeRecipeLinks(string html);

        public abstract Task<IList<RecipeMetaDtoV2>> ScrapeRecipesAsync(int amount = -1);
    }
}
