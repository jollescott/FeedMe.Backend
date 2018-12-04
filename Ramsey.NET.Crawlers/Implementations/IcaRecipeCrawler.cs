using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Extensions;

namespace Ramsey.NET.Crawlers.Implementations
{
    public class IcaRecipeCrawler : AIcaRecipeCrawler
    {
        private readonly string QUERY = "https://www.ica.se/templates/ajaxresponse.aspx?id=12&ajaxFunction=RecipeListMdsa&mdsarowentityid=&sortbymetadata=Relevance&start=0&num=1000";
        private readonly HttpClient _httpClient;

        public IcaRecipeCrawler()
        {
            _httpClient = new HttpClient();
        }

        public override async Task<List<string>> ScrapeLinksAsync()
        {
            var response = await _httpClient.GetAsync(QUERY);

            if(response.IsSuccessStatusCode)
            {
                HtmlDocument document = new HtmlDocument();
                var html = await response.Content.ReadAsSwedishStringAsync();

                document.LoadHtml(html);

                var links = document.DocumentNode.SelectNodes("//a[@class = \"js-track-listing-recipe\"]");
                return links.Select(x => x.Attributes["href"].Value).ToList();
            }
            else
            {
                return null;
            }
        }

        public override async Task<RecipeDto> ScrapeRecipeAsync(string url)
        {
            var doc = new HtmlDocument();
            var client = new HttpClient();
            var dto = new RecipeDto();

            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsSwedishStringAsync();

            doc.LoadHtml(html);

            dto.Name = doc.DocumentNode.SelectSingleNode("//h1[@class = \"recipepage__headline\"]").InnerText;
            var img = doc.DocumentNode.SelectSingleNode("/html/body/form/div[4]/div/div[2]/main/header/div/div[2]/div/div");
            dto.Image = Regex.Match(img.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value.Replace("&#39;", string.Empty);

            var ingredientList = doc.DocumentNode.SelectSingleNode("//ul[@class= \"ingredients__list\"]");
            var ingredients = ingredientList.SelectNodes(".//span[@class = \"ingredient\"]")
                .Select(x => x.InnerText)
                .Select(z => SortIngredient(z)).ToList();

            dto.Ingredients = ingredients.ToList();

            var directionsParent = doc.DocumentNode.SelectSingleNode("//*[@id=\"recipe-howto-steps\"]");
            var directions = directionsParent.SelectNodes(".//li").Select(x => x.InnerText).ToList();
            //var directions = directionsList.SelectNodes("//div[@class=\"cooking-step__content__instruction\"]").Select(x => x.InnerText).ToList();

            //dto.Directions = directions;

            return dto;
        }

        private string SortIngredient(string z)
        {
            var words = z.Split(' ');

            if (words.Count() <= 2)
            {
                return z;
            }
            else
            {
                return new StringBuilder().Append(words[words.Count() - 2]).Append(words.Last()).ToString();
            }
        }

        public override async Task<List<RecipeDto>> ScrapeRecipesAsync()
        {
            var links = await ScrapeLinksAsync();
            var recipes = new List<RecipeDto>();

            var step = links.Count / 10;
            var index = 0;

            for(var i = 0; i < step; i++)
            {
                var cLinks = links.Skip(index).Take(step);
                index += step;

                var rTasks = cLinks.Select(x => ScrapeRecipeAsync(x));
                var sRecipes = await Task.WhenAll(rTasks);

                recipes.AddRange(sRecipes);
            }

            return recipes;
        }
    }
}
