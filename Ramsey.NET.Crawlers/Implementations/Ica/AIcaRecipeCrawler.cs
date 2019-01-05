using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;

namespace Ramsey.NET.Crawlers.Implementations.Ica
{
    public abstract class AIcaRecipeCrawler : IRecipeCrawler
    {
        public abstract Task<Dictionary<string, bool>> ScrapeRecipesAsync(IRecipeManager recipeManager, int amount = 50);

        abstract public Task<List<string>> ScrapeLinksAsync(int amount);
        abstract public Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false);
    }
}
