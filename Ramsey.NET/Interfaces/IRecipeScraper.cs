using Ramsey.Shared.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramsey.NET.Interfaces
{
    public interface IRecipeScraper
    {
        Task<List<RecipeDto>> ScrapeRecipiesAsync(List<string> ingredients);
    }
}
