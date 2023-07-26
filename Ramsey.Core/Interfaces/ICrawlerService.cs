using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Enums;
using System.Threading.Tasks;

namespace FeedMe.Crawler.Interfaces
{
    public interface ICrawlerService
    {
        Task<RecipeDtoV2> ScrapeRecipeAsync(string url, RecipeProvider provider);
        Task ReindexProviderAsync(RecipeProvider provider);
        void StartIndexUpdate();
    }
}
