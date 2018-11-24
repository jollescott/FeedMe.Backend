using Ramsey.NET.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Interfaces
{
    public interface IRecipeScraper
    {
        Task<List<RecipeDto>> ScrapeRecipiesAsync(List<string> ingredients);
    }
}
