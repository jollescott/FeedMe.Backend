using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ramsey.NET.Shared.Interfaces;

namespace Ramsey.NET.Crawlers.Interfaces
{
    public interface IRecipeCrawler
    {
        Task<Dictionary<string, bool>> ScrapeRecipesAsync(IRecipeManager recipeManager, int amount = 50);
        Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false);
    }
}
