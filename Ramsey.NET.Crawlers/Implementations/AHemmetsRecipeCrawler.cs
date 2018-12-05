using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Crawlers.Implementations
{
    public abstract class AHemmetsRecipeCrawler : IRecipeCrawler
    {
        public abstract Task<int> GetRecipeCountAsync();

        public abstract Task<List<RecipeDto>> ScrapePagesAsync(int count, int offset);

        public abstract Task<List<RecipeDto>> ScrapePageAsync(int offset);
        public abstract Task<RecipeDto> ScrapeRecipeAsync(string url, bool includeAll = false);

        public abstract int ScrapeRecipeCount(string html);
        public abstract IEnumerable<string> ScapeRecipeLinks(string html);

        public abstract Task<List<RecipeDto>> ScrapeRecipesAsync();
    }
}
