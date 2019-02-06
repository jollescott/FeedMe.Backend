using System.Collections.Generic;
using System.Threading.Tasks;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;

namespace Ramsey.NET.Crawlers.Interfaces
{
    public interface IRecipeCrawler
    {
        Task ScrapeRecipesAsync(IRecipeManager recipeManager);
        Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false);
    }
}
