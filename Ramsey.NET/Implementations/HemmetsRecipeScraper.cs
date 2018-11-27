using HtmlAgilityPack;
using Ramsey.NET.Dto;
using Ramsey.NET.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Implementations
{
    public class HemmetsRecipeScraper : IRecipeScraper
    {
        private readonly string HEMMETS_ROOT = "https://kokboken.ikv.uu.se/";
        private readonly HttpClient _client;

        public HemmetsRecipeScraper()
        {
            _client = new HttpClient();

            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0");
            _client.DefaultRequestHeaders.Add("Accept-Language", "sv-SE,sv;q=0.8,en-US;q=0.5,en;q=0.3");
            _client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            _client.DefaultRequestHeaders.Add("Referer", "https://kokboken.ikv.uu.se/sok.php");
            //_client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            _client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
        }

        public async Task<List<RecipeDto>> ScrapeRecipiesAsync(List<string> ingredients)
        {
            StringBuilder query = new StringBuilder();

            foreach (var ing in ingredients)
            {
                query = query.Append(ing).Append(" ");
            }

            var postData = $"search_text={query.ToString()}&dummy=&search_type=all&rec_cats%5B%5D=all&submit_search=S%F6k&recid=&offset=0&searchid=";

            var response = await _client.PostAsync("https://kokboken.ikv.uu.se/sok.php", new StringContent(postData, Encoding.GetEncoding(1252), "application/x-www-form-urlencoded"));
            var bytes = await response.Content.ReadAsByteArrayAsync();
            var recipe_html = Encoding.GetEncoding(1252).GetString(bytes);

            if (recipe_html != string.Empty)
            {
                var home_document = new HtmlDocument();
                home_document.LoadHtml(recipe_html);

                var main_wrapper = home_document.DocumentNode.Descendants().Where(x => x.Id.Equals("mainwrapper")).FirstOrDefault();

                if (main_wrapper != null)
                {
                    var main_content = main_wrapper.Descendants().Where(x => x.Id.Equals("maincontent")).FirstOrDefault();
                    var tables = main_content.SelectNodes(".//table");

                    var recipe_table = tables[2];
                    var recipe_links = recipe_table.SelectNodes(".//tr/td/strong/a").Select(x => x.Attributes["href"].Value).ToList();

                    var tasks = recipe_links.Select(x => LoadRecipeFromLinkAsync(new StringBuilder()
                        .Append(HEMMETS_ROOT)
                        .Append(x)
                        .ToString()));

                    var recipes = await Task.WhenAll(tasks);
                    var sortedRecipes = recipes.Where(x => x.Ingredients.All(y => ingredients.Any(s => s.Equals(y, StringComparison.OrdinalIgnoreCase))));

                    return sortedRecipes.ToList();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private async Task<RecipeDto> LoadRecipeFromLinkAsync(string link)
        {
            System.Diagnostics.Debug.WriteLine(link);
            var recipeDto = new RecipeDto();

            var bytes = await _client.GetByteArrayAsync(link);
            var html = Encoding.GetEncoding(1252).GetString(bytes);

            var recipe_document = new HtmlDocument();
            recipe_document.LoadHtml(html);

            var recept_info = recipe_document.DocumentNode.SelectSingleNode("//div[@class=\"receptinfo\"]");
            recipeDto.Name = recept_info.SelectSingleNode("//h1").InnerText;

            var recept_bild = recipe_document.DocumentNode.SelectSingleNode("//div[@class=\"receptbild\"]/img");
            recipeDto.Image = new StringBuilder().Append(HEMMETS_ROOT).Append(recept_bild.Attributes["src"].Value).ToString();

            var directions = recipe_document.DocumentNode.SelectNodes("//div[@class=\"receptrightcol\"]/table/tr")
                .Skip(1)
                .Select(x => WebUtility.HtmlDecode(x.InnerText?.Trim()))
                .Select(y => y.Replace("\n", ""))
                .Select(z => z.Replace("\t", ""))
                .ToList();

            var ingredients = recipe_document.DocumentNode.SelectNodes("//div[@class=\"receptleftcol\"]/table/tr/td")
                .Where(x => !x.Attributes.Contains("align"))
                .Select(y => WebUtility.HtmlDecode(y.InnerText?.Trim()))
                .Select(z => z.Replace("\n", ""))
                .Select(d => d.Replace("\t", ""))
                .ToList();

            recipeDto.Ingredients = ingredients;
            recipeDto.Directions = directions;

            recipeDto.Source = link;

            return recipeDto;
        }
    }
}
