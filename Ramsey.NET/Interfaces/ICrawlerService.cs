using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Interfaces
{
    public interface ICrawlerService
    {
        Task UpdateIndexAsync();
        Task<RecipeDtoV2> ScrapeRecipeAsync(string url, RecipeProvider provider);
    }
}
