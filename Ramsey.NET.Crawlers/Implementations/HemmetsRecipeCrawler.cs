using HtmlAgilityPack;
using Ramsey.NET.Crawlers.Interfaces;
using Ramsey.NET.Crawlers.Misc;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Crawlers.Implementations
{
    public class HemmetsRecipeCrawler : AHemmetsRecipeCrawler
    {
        private readonly HemmetsHttpClient _client;

        private readonly string RECIPE_LIST_URL = "https://kokboken.ikv.uu.se/receptlista.php?cat=0";
        private readonly string HEMMETS_ROOT = "https://kokboken.ikv.uu.se/";
        private int _currentIndex = 0;

        public HemmetsRecipeCrawler()
        {
            _client = new HemmetsHttpClient();
        }

        public override async Task<int> GetRecipeCountAsync()
        {
            var httpResponse = await _client.GetAsync(RECIPE_LIST_URL);
            
            if(httpResponse.IsSuccessStatusCode)
            {
                var html = await httpResponse.Content.ReadAsSwedishStringAsync();
                return ScrapeRecipeCount(html);
            }
            else
            {
                return 0;
            }
        }

        public override int ScrapeRecipeCount(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var main_content = doc.DocumentNode.Descendants().Where(x => x.Id.Equals("maincontent")).FirstOrDefault();
            var tables = main_content.SelectNodes("//table");

            var nav_table = tables[1];
            var tds = nav_table.SelectNodes("//td");
            var fullText = tds.Last().InnerText;

            var numText = fullText.Split(' ')[0].Split('(').Last();
            var doub = Double.Parse(numText);

            return (int)Math.Ceiling(doub);
        }

        public override async Task<RecipeDto> ScrapeRecipeAsync(string url)
        {
            System.Diagnostics.Debug.WriteLine(url);
            var recipeDto = new RecipeDto();

            var client = new HemmetsHttpClient();

            var bytes = await client.GetByteArrayAsync(url);
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

            recipeDto.Ingredients = new HashSet<string>(ingredients).ToList();
            recipeDto.Directions = directions;

            recipeDto.Source = url;
            recipeDto.Owner = RecipeProvider.Hemmets;

            _currentIndex++;
            recipeDto.RecipeID = "HEMMETS" + _currentIndex.ToString();

            return recipeDto;
        }

        public override async Task<List<RecipeDto>> ScrapeRecipesAsync()
        {
            var recipeCount = await GetRecipeCountAsync();
            var pageCount = (int)Math.Ceiling((double)recipeCount / 20);

            return await ScrapePagesAsync(pageCount, 20);
        }


        public override async Task<List<RecipeDto>> ScrapePagesAsync(int count, int offset)
        {
            var tasks = new List<Task<List<RecipeDto>>>();
            var allRecipes = new List<RecipeDto>();

            for (var i = 0; i < count; i++)
            {
                await Task.Delay(1000);
                var recipes = await ScrapePageAsync(offset * i);
                recipes.ToList().ForEach(x => allRecipes.Add(x));
            }

            return new HashSet<RecipeDto>(allRecipes).ToList();
        }


        public override async Task<List<RecipeDto>> ScrapePageAsync(int offset)
        {
            var postData = $"offset={offset}";

            var response = await _client.PostAsync(RECIPE_LIST_URL, new StringContent(postData, Encoding.GetEncoding(1252), "application/x-www-form-urlencoded"));
            var html = await response.Content.ReadAsSwedishStringAsync();
            var links = ScapeRecipeLinks(html);

            var tasks = links.Select(x => ScrapeRecipeAsync(new StringBuilder()
                       .Append(HEMMETS_ROOT)
                       .Append(x.Split('&')[0])
                       .ToString()));

            var recipes = await Task.WhenAll(tasks);
            return recipes.ToList();
        }

        public override IEnumerable<string> ScapeRecipeLinks(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var main_content = doc.DocumentNode.Descendants().Where(x => x.Id.Equals("maincontent")).FirstOrDefault();
            var tables = main_content.SelectNodes("//table");

            var recipe_table = tables[0];
            var recipe_links = recipe_table.SelectNodes(".//tr/td/strong/a").Select(x => x.Attributes["href"].Value).ToList();
            return recipe_links;
        }
    }
}
