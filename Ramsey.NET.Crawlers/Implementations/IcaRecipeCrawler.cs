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
        private readonly string QUERY = "https://www.ica.se/templates/ajaxresponse.aspx?id=12&ajaxFunction=RecipeListMdsa&mdsarowentityid=&sortbymetadata=Relevance&start=0&num=50";
        private readonly HttpClient _httpClient;
        private int _currentIndex = 0;

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

        public override async Task<RecipeDto> ScrapeRecipeAsync(string url, bool includeAll = false)
        {
            var doc = new HtmlDocument();
            var client = new HttpClient();
            var dto = new RecipeDto();

            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsSwedishStringAsync();

            doc.LoadHtml(html);

            dto.Name = doc.DocumentNode.SelectSingleNode("//h1[@class = \"recipepage__headline\"]").InnerText;

            try
            {
                var square = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'recipe-image-square__image')]");
                var hero = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'hero__image__background')]");
                var imgurl = string.Empty;

                if(square == null)
                {
                    imgurl = Regex.Match(hero.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value;
                }
                else
                {
                    imgurl = Regex.Match(square.GetAttributeValue("style", ""), @"(?<=url\()(.*)(?=\))").Groups[1].Value;
                }

                dto.Image = imgurl.Replace("&#39;", string.Empty);
            }
            catch(NullReferenceException)
            {

            }

            var ingredientList = doc.DocumentNode.SelectSingleNode("//ul[@class= \"ingredients__list\"]");
            var ingredients = ingredientList.SelectNodes(".//span[@class = \"ingredient\"]")
                .Select(x => x.InnerText)
                .Select(z => SortIngredient(z)).ToList();

            dto.Ingredients = ingredients.ToList();

            if(includeAll)
            {
                var directionsParent = doc.DocumentNode.SelectSingleNode("//*[@id=\"recipe-howto-steps\"]");
                var directions = directionsParent.SelectNodes(".//li").Select(x => x.InnerText).ToList();
                dto.Directions = directions;
            }
            //var directions = directionsList.SelectNodes("//div[@class=\"cooking-step__content__instruction\"]").Select(x => x.InnerText).ToList();

            //dto.Directions = directions;

            dto.RecipeID = "ICA" + _currentIndex.ToString();
            _currentIndex++;

            dto.Source = url;
            dto.Owner = RecipeProvider.ICA;
            dto.OwnerLogo = "https://upload.wikimedia.org/wikipedia/commons/7/74/ICA-logotyp.png";

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

        public override async Task<List<RecipeMetaDto>> ScrapeRecipesAsync()
        {
            var links = await ScrapeLinksAsync();
            var recipes = new List<RecipeMetaDto>();

            var step = links.Count / 10;
            var index = 0;

            for(var i = 0; i < step; i++)
            {
                var cLinks = links.Skip(index).Take(step);
                index += step;

                var rTasks = cLinks.Select(x => ScrapeRecipeAsync(x));
                var sRecipes = await Task.WhenAll(rTasks);
                var sMetas = sRecipes.Select(x => (RecipeMetaDto)x);

                recipes.AddRange(sMetas);
            }

            return recipes;
        }
    }
}
