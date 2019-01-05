using System.Collections.Generic;
using System.Threading.Tasks;
using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;

namespace Ramsey.NET.Crawlers.Implementations.Mathem
{
    public abstract class AMathemCrawler : IRecipeCrawler
    {
        public abstract Task<Dictionary<string, bool>> ScrapeRecipesAsync(IRecipeManager recipeManager, int amount = 1000);

        public abstract Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false);

        public abstract MathemRecipeDetails GetDetailsFromJson(string json);
    }
}