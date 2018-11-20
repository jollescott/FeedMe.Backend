using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GusteauSharp.Dto;
using GusteauSharp.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Dto;
using Ramsey.NET.Models;

namespace Ramsey.NET.Controllers
{
    [Route("recipe")]
    public class RecipeController : Controller
    {
        private readonly RamseyContext _ramseyContext;
        private CookieContainer _cookieContainer;
        private HttpClientHandler _httpHandler;
        private readonly HttpClient _httpClient;

        public RecipeController(RamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
            _cookieContainer = new CookieContainer();
            _httpHandler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = true, CookieContainer = _cookieContainer };
            _httpClient = new HttpClient(_httpHandler);
            _httpClient.BaseAddress = new Uri("https://kokboken.ikv.uu.se/");
        }

        [Route("upload")]
        [HttpPost]
        public IActionResult Upload([FromBody]RecipeUploadDto recipeDto)
        {
            if (recipeDto == null)
                return new StatusCodeResult(200);

            Recipe recipe = new Recipe
            {
                Name = recipeDto.Title,
                Rating = recipeDto.Rating,
                
                Fat = recipeDto.Fat,
                Protein = recipeDto.Protein,
                Sodium = recipeDto.Sodium
            };

            if (recipeDto.Desc != null)
                recipe.Desc = recipeDto.Desc;

            _ramseyContext.Recipes.Add(recipe);
            _ramseyContext.SaveChanges();
            
            if(recipeDto.Directions != null)
            {
                foreach (var direction in recipeDto.Directions)
                {
                    var recipeDirection = new RecipeDirection();
                    recipeDirection.Instruction = direction;
                    recipeDirection.RecipeID = recipe.RecipeID;

                    _ramseyContext.RecipeDirections.Add(recipeDirection);
                }
            }

            _ramseyContext.SaveChanges();

            if(recipeDto.Categories != null)
            {
                foreach (var category in recipeDto.Categories)
                {
                    var recipeCategory = new RecipeCategory();
                    recipeCategory.Name = category;
                    recipeCategory.RecipeID = recipe.RecipeID;

                    _ramseyContext.RecipeCategories.Add(recipeCategory);
                }
            }

            _ramseyContext.SaveChanges();

            foreach(var ingredientLine in recipeDto.Ingredients)
            {
                var parts = ingredientLine.Split(' ');

                double quantity;

                if(parts[0].Contains('/'))
                {
                    var nums = parts[0].Split('/');

                    double num1, num2;
                    if (!double.TryParse(nums[0], out num1))
                        num1 = 1;

                    if (!double.TryParse(nums[1], out num2))
                        num2 = 1;

                    parts[0] = (num1 / num2).ToString();
                }

                if (!double.TryParse(parts[0], out quantity))
                    quantity = 1;

                string unit;

                try
                {
                    unit = parts[1];
                }
                catch(IndexOutOfRangeException)
                {
                    unit = "";
                }
 
                string name = string.Join(' ', parts.Skip(2));

                Ingredient ingredient;

                if(_ramseyContext.Ingredients.Where(x => x.Name.Equals(name)).FirstOrDefault() == null)
                {
                    ingredient = new Ingredient
                    {
                        Name = name
                    };

                    _ramseyContext.Ingredients.Add(ingredient);
                    _ramseyContext.SaveChanges();
                }
                else
                {
                    ingredient = _ramseyContext.Ingredients.Where(x => x.Name.Equals(name)).First();
                }

                var part = new RecipePart();

                part.Unit = unit;
                part.Quantity = quantity;
                part.IngredientID = ingredient.IngredientID;
                part.RecipeID = recipe.RecipeID;

                _ramseyContext.RecipeParts.Add(part);
            }

            _ramseyContext.SaveChanges();

            return new StatusCodeResult(200);
        }

        [Route("suggest")]
        [HttpPost]
        public async Task<IActionResult> SuggestAsync([FromBody]List<string> ingredients)
        {
            await _httpClient.GetAsync("/sok.php");

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://kokboken.ikv.uu.se/sok.php"))
            {
                request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
                request.Headers.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                request.Headers.TryAddWithoutValidation("Accept-Language", "sv-SE,sv;q=0.8,en-US;q=0.5,en;q=0.3");
                request.Headers.TryAddWithoutValidation("Cookie", "_ga=GA1.2.1641912861.1542728984; _gid=GA1.2.984168996.1542728984");
                request.Headers.TryAddWithoutValidation("Referer", "https://kokboken.ikv.uu.se/sok.php");
                request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0");
                request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                request.Content = new StringContent("search_text=test&dummy=&search_type=all&rec_cats", Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await _httpClient.SendAsync(request);

                var html = await response.Content.ReadAsStringAsync();
                return Content(html);
            }

            /*var website = new HtmlDocument();
            website.Load(html);

            var main_wrapper = website.DocumentNode.Descendants().Where(x => x.Id.Equals("mainwrapper")).FirstOrDefault();
            var main_content = main_wrapper.Descendants().Where(x => x.Id.Equals("maincontent")).FirstOrDefault();

            var table = main_wrapper.SelectNodes("//table[@id]")[1];

            var nodes = table.SelectNodes("/tr/td/strong/a");

            return Json(nodes);*/
        }
    }
}