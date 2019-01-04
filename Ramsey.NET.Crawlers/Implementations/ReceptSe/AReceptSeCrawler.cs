using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Crawlers.Implementations.ReceptSe
{
    public abstract class AReceptSeCrawler : IRecipeCrawler
    {
        public abstract Task<Dictionary<string, bool>> ScrapeRecipesAsync(IRecipeManager recipeManager, int amount = -1);

        public abstract Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false);

        public abstract Task<int> GetRecipeCountAsync();

        public abstract Task<IEnumerable<string>> GetPageLinksAsync(int index);

        public abstract string GetRecipeTitle(HtmlDocument document);
        public abstract string GetReceptSeLogo(HtmlDocument document);

        public abstract string GetRecipeLogo(HtmlDocument document);

        public abstract IEnumerable<RecipePartDtoV2> GetRecipeParts(HtmlDocument document, string recipeid);
        public abstract IEnumerable<string> GetRecipeDirections(HtmlDocument document);
    }
}