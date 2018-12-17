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
using System.Text.RegularExpressions;
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

        public override async Task<RecipeDtoV2> ScrapeRecipeAsync(string url, bool includeAll = false)
        {
            System.Diagnostics.Debug.WriteLine(url);
            var recipeDto = new RecipeDtoV2();

            var client = new HemmetsHttpClient();

            var bytes = await client.GetByteArrayAsync(url);
            var html = Encoding.GetEncoding(1252).GetString(bytes);

            var recipe_document = new HtmlDocument();
            recipe_document.LoadHtml(html);

            var recept_info = recipe_document.DocumentNode.SelectSingleNode("//div[@class=\"receptinfo\"]");
            recipeDto.Name = recept_info.SelectSingleNode("//h1").InnerText;

            var recept_bild = recipe_document.DocumentNode.SelectSingleNode("//div[@class=\"receptbild\"]/img");
            recipeDto.Image = new StringBuilder().Append(HEMMETS_ROOT).Append(recept_bild.Attributes["src"].Value).ToString();

            if(includeAll)
            {
                var directions = recipe_document.DocumentNode.SelectNodes("//div[@class=\"receptrightcol\"]/table/tr")
                .Skip(1)
                .Select(x => WebUtility.HtmlDecode(x.InnerText?.Trim()))
                .Select(y => y.Replace("\n", ""))
                .Select(z => z.Replace("\t", ""))
                .ToList();

                recipeDto.Directions = directions;
            }

            var ingredients_root = recipe_document.DocumentNode.SelectSingleNode("//div[@class=\"receptleftcol\"]/table");
            var table_groups = ingredients_root.SelectNodes(".//tr").ToList().Skip(1);
            var ingredient_groups = table_groups.Select(x => x.ChildNodes.Where(z => z.Name.Equals("td")).Where(l => l.InnerText != "-").Select(y => y.InnerText)).ToList().Where(x => x.Count() > 0);

            var recipeParts = new List<RecipePartDtoV2>();
            foreach(var ing_group in ingredient_groups)
            {
                var part = new RecipePartDtoV2();
                part.IngredientID = ing_group.First().ToLower();

                var unitGroup = ing_group.Last().Split(' ');

                if(unitGroup.Count() > 0)
                {
                    if(float.TryParse(unitGroup.First(), out float quantity))
                    {
                        part.Quantity = quantity;
                        part.Unit = unitGroup.Last();
                    }
                    else
                    {
                        part.Quantity = 0;
                        part.Unit = ing_group.Last();
                    }

                    recipeParts.Add(part);
                }
            }

            recipeDto.RecipeParts = recipeParts;
            recipeDto.Ingredients = recipeParts.Select(x => x.IngredientID).ToList();

            recipeDto.Source = url;
            recipeDto.Owner = RecipeProvider.Hemmets;

            _currentIndex++;
            recipeDto.RecipeID = "HEMMETS" + url.Split('=').Last();
            recipeDto.OwnerLogo = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dc/UU_logo.jpg/628px-UU_logo.jpg";

            return recipeDto;
        }

        public override async Task<List<RecipeMetaDtoV2>> ScrapeRecipesAsync(int amount = -1)
        {
            double recipeCount = await GetRecipeCountAsync();
            recipeCount = amount > -1 ? amount : recipeCount;
            var pageCount = (int)Math.Ceiling(recipeCount / 20);

            return await ScrapePagesAsync(pageCount, 20);
        }


        public override async Task<List<RecipeMetaDtoV2>> ScrapePagesAsync(int count, int offset)
        {
            var allRecipes = new List<RecipeMetaDtoV2>();

            for (var i = 0; i < count; i++)
            {
                await Task.Delay(2000);
                var recipes = await ScrapePageAsync(offset * i);
                recipes.ToList().ForEach(x => allRecipes.Add(x));
            }

            return new HashSet<RecipeMetaDtoV2>(allRecipes).ToList();
        }


        public override async Task<List<RecipeMetaDtoV2>> ScrapePageAsync(int offset)
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
            var metas = recipes.Select(x => (RecipeMetaDtoV2)x);

            return metas.ToList();
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
