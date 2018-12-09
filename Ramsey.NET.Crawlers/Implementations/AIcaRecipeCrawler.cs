using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Crawlers.Implementations
{
    public abstract class AIcaRecipeCrawler : IRecipeCrawler
    {
        abstract public Task<List<RecipeMetaDto>> ScrapeRecipesAsync();

        abstract public Task<List<string>> ScrapeLinksAsync();
        abstract public Task<RecipeDto> ScrapeRecipeAsync(string url, bool includeAll = false);
    }
}
