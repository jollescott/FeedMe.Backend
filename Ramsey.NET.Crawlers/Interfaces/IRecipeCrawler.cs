using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Crawlers.Interfaces
{
    public interface IRecipeCrawler
    {
        Task<List<RecipeMetaDto>> ScrapeRecipesAsync(int amount = 50);
        Task<RecipeDto> ScrapeRecipeAsync(string url, bool includeAll = false);
    }
}
