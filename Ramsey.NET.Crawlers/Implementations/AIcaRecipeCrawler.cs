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
        abstract public Task<List<RecipeMetaDtoV2>> ScrapeRecipesAsync(int amount = 50);

        abstract public Task<List<string>> ScrapeLinksAsync(int amount);
        abstract public Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false);
    }
}
