using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Crawlers.Interfaces
{
    public interface IRecipeCrawler
    {
        Task<IList<RecipeMetaDtoV2>> ScrapeRecipesAsync(int amount = 50);
        Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false);
    }
}
