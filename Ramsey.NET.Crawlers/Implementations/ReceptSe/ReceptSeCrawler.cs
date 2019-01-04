using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Extensions;

namespace Ramsey.NET.Crawlers.Implementations.ReceptSe
{
    public class ReceptSeCrawler : AReceptSeCrawler
    {
        private static readonly string ReceptSeBaseUrl = "http://recept.se/recept";
        private readonly HttpClient _httpClient = new HttpClient();
        
        public override async Task<Dictionary<string, bool>> ScrapeRecipesAsync(IRecipeManager recipeManager,int amount = -1)
        {
            var count = await GetRecipeCountAsync();

            if (amount > 0) count = amount; 

            for (var i = 1; i < count; i++)
            {
                var links = await GetPageLinksAsync(i + 1);
                var tasks = links.Select(x => ScrapeRecipeAsync(x));
                var recipeDtoV2s = await Task.WhenAll(tasks);

                var submitRecipes = recipeDtoV2s.Where(x => x.RecipeParts != null).Select(recipeDtoV2 => (RecipeMetaDtoV2) recipeDtoV2);

                foreach (var recipe in submitRecipes)
                    await recipeManager.UpdateRecipeMetaAsync(recipe);

                await recipeManager.SaveRecipeChangesAsync();
            }

            return new Dictionary<string, bool>();
        }

        public override async Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false)
        {
            var recipeDto = new RecipeDtoV2();
            
            var response = await _httpClient.GetAsync(url);
            var html = await response.Content.ReadAsSwedishStringAsync();

            var document = new HtmlDocument();
            document.LoadHtml(html);

            recipeDto.Name = GetRecipeTitle(document);
            recipeDto.Owner = RecipeProvider.ReceptSe;
            recipeDto.Source = url;
            recipeDto.RecipeParts = GetRecipeParts(document);
            recipeDto.Ingredients = recipeDto.RecipeParts?.Select(x => x.IngredientID);
            recipeDto.Image = GetRecipeLogo(document);
            recipeDto.OwnerLogo = GetReceptSeLogo(document);

            if (includeAll)
                recipeDto.Directions = GetRecipeDirections(document);

            recipeDto.RecipeID = "RSE" + url.Split('/').Last();

            return recipeDto;
        }

        public override async Task<int> GetRecipeCountAsync()
        {
            var response = await _httpClient.GetAsync(ReceptSeBaseUrl);
            var html = await response.Content.ReadAsSwedishStringAsync();

            var document = new HtmlDocument();
            document.LoadHtml(html);

            var pagesContainer =
                document.DocumentNode.SelectSingleNode("//div[@class=\"views-aller-custom-pager-current-pages-span\"]");

            var countText = pagesContainer.Descendants().Last(x => x.Name == "a").InnerText;
            int.TryParse(countText, out var count);

            return count;
        }

        public override async Task<IEnumerable<string>> GetPageLinksAsync(int index)
        {
            var url = "http://recept.se/recept?page=" + index;
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var html = await response.Content.ReadAsSwedishStringAsync();
            
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var links = document.DocumentNode.SelectNodes("//div[@class=\"views-field views-field-title\"]/span/a")
                .Select(x => x.GetAttributeValue("href", string.Empty).ToLower()).ToList();

            return links.Take(18);
        }

        public override string GetRecipeTitle(HtmlDocument document)
        {
            var title = document.DocumentNode
                .SelectSingleNode("//h1[@class=\"fn\"]")
                .InnerText;

            return title;
        }

        public override string GetReceptSeLogo(HtmlDocument document)
        {
            var logoUrl = document.DocumentNode.SelectSingleNode("//img[@id=\"logo\"]").GetAttributeValue("src", "");
            return logoUrl;
        }

        public override string GetRecipeLogo(HtmlDocument document)
        {
            var photo = document.DocumentNode.SelectSingleNode("//img[@class=\"photo\"]").GetAttributeValue("src", "");
            return photo;
        }

        public override IEnumerable<RecipePartDtoV2> GetRecipeParts(HtmlDocument document)
        {
            var ingredientDivs = document.DocumentNode.SelectNodes("//div[@class=\"ingredient\"]");
            var ingredients = new List<RecipePartDtoV2>();

            if (ingredientDivs == null) return null;

            foreach (var ingredientDiv in ingredientDivs)
            {
                var stringParts = ingredientDiv.Descendants("div")
                    .Take(2)
                    .Select(x => x.InnerText)
                    .ToList();

                var amount = Regex.Replace(stringParts.First(), @"\t|\n|\r", "").TrimSpacesBetweenString();
                var name = stringParts.Last().Trim();
                
                if(amount == null && name == null) continue;

                var recipePart = new RecipePartDtoV2 {IngredientID = name};

                if (!string.IsNullOrEmpty(amount))
                {
                    var parts = amount.Split(' ');
                    var quantity_str = parts.First().Trim();
                    var unit = parts.Last();

                    float.TryParse(quantity_str, out var quantity);
                    
                    recipePart.Unit = unit.Trim();
                    recipePart.Quantity = quantity;
                }          
                
                ingredients.Add(recipePart);
            }

            return ingredients;
        }

        public override IEnumerable<string> GetRecipeDirections(HtmlDocument document)
        {
            var directions = document.DocumentNode.SelectNodes("//ol[@class=\"instructions\"]/li/span")
                .Select(x => x.InnerText);

            return directions;
        }
    }

    public static class StringExt
    {
        public static string TrimSpacesBetweenString(this string s)
        {
            return string.Join(" ", s.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(w => w.Trim()));
        }
    }
}