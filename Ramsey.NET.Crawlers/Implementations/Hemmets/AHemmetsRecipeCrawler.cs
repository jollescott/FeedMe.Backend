using Ramsey.NET.Crawlers.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;

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

        public abstract Task ScrapeRecipesAsync(IRecipeManager recipeManager, int amount = -1);
    }
}
