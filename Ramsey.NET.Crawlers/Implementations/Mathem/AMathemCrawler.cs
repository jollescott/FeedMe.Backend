using System.Collections.Generic;
using System.Threading.Tasks;
using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Crawlers.Implementations.Mathem
{
    public abstract class AMathemCrawler : IRecipeCrawler
    {
        public abstract Task<IList<RecipeMetaDtoV2>> ScrapeRecipesAsync(int amount = 50);

        public abstract Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false);

        public abstract MathemRecipeDetails GetDetailsFromJson(string json);
    }
}