using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Crawlers.Interfaces
{
    public interface IHemmetsRecipeCrawler : IRecipeCrawler
    {
        Task<int> GetRecipeCountAsync();

        Task<List<RecipeDto>> ScrapePagesAsync(int count, int offset);

        Task<List<RecipeDto>> ScrapePageAsync(int offset);
        Task<RecipeDto> ScrapeRecipeAsync(string url);

        int ScrapeRecipeCount(string html);
        IEnumerable<string> ScapeRecipeLinks(string html);
    }
}
