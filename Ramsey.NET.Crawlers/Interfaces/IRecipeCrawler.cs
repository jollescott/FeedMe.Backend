using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Crawlers.Interfaces
{
    public interface IRecipeCrawler
    {
        Task<List<RecipeDto>> ScrapeRecipesAsync();
        Task<RecipeDto> ScrapeRecipeAsync(string url, bool includeAll = false);
    }
}
