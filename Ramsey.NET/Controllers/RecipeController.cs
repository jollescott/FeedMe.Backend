using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CurlThin;
using CurlThin.Enums;
using CurlThin.Native;
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
        private readonly string HEMMETS_ROOT = "https://kokboken.ikv.uu.se/";

        public RecipeController(RamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
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
        public IActionResult Suggest([FromBody]List<string> ingredients)
        {
            // curl_global_init() with default flags.
            var global = CurlNative.Init();
            string html = string.Empty;

            // curl_easy_init() to create easy handle.
            var easy = CurlNative.Easy.Init();
            try
            {
                StringBuilder query = new StringBuilder();

                foreach(var ing in ingredients)
                {
                    query = query.Append(ing).Append(' ');
                }
                
                var postData = $"search_text={query.ToString()}&dummy=&search_type=all&rec_cats\"%\"5B\"%\"5D=all&submit_search=S\"%\"F6k&recid=&offset=0&scode=YWxsI2FsbCN0ZXN0&searchid=";

                CurlNative.Easy.SetOpt(easy, CURLoption.URL, HEMMETS_ROOT + "sok.php");

                // This one has to be called before setting COPYPOSTFIELDS.
                CurlNative.Easy.SetOpt(easy, CURLoption.POSTFIELDSIZE, Encoding.ASCII.GetByteCount(postData));
                CurlNative.Easy.SetOpt(easy, CURLoption.COPYPOSTFIELDS, postData);
                CurlNative.Easy.SetOpt(easy, CURLoption.CAINFO, CurlResources.CaBundlePath);

                var stream = new MemoryStream();
                CurlNative.Easy.SetOpt(easy, CURLoption.WRITEFUNCTION, (data, size, nmemb, user) =>
                {
                    var length = (int)size * (int)nmemb;
                    var buffer = new byte[length];
                    Marshal.Copy(data, buffer, 0, length);
                    stream.Write(buffer, 0, length);
                    return (UIntPtr)length;
                });

                var result = CurlNative.Easy.Perform(easy);
                html = Encoding.GetEncoding(1252).GetString(stream.ToArray());
            }
            finally
            {
                easy.Dispose();

                if (global == CURLcode.OK)
                {
                    CurlNative.Cleanup();
                }
            }

            if (html != string.Empty)
            {
                var home_document = new HtmlDocument();
                home_document.LoadHtml(html);

                var main_wrapper = home_document.DocumentNode.Descendants().Where(x => x.Id.Equals("mainwrapper")).FirstOrDefault();

                if(main_wrapper != null)
                {
                    var main_content = main_wrapper.Descendants().Where(x => x.Id.Equals("maincontent")).FirstOrDefault();
                    var tables = main_content.SelectNodes("//table");

                    var recipe_table = tables[2];
                    var recipe_links = recipe_table.SelectNodes("//tr/td/strong/a").Select(x => x.Attributes["href"].Value).ToList();


                    var recipes = recipe_links.Select(x => LoadRecipeFromLink(new StringBuilder()
                        .Append(HEMMETS_ROOT)
                        .Append(x)
                        .ToString()));

                    return Json(recipes);
                }
                else
                {
                    return StatusCode(503);
                }
            }
            else
            {
                return StatusCode(503);
            }
        }

        private RecipeDto LoadRecipeFromLink(string link)
        {
            var recipeDto = new RecipeDto();
            HtmlWeb web = new HtmlWeb();
            var recipe_document = web.Load(link);


            var recept_info = recipe_document.DocumentNode.SelectSingleNode("//div[@class=\"receptinfo\"]");
            recipeDto.Name = recept_info.SelectSingleNode("//h1").InnerText;

            var recept_bild = recipe_document.DocumentNode.SelectSingleNode("//div[@class=\"receptbild\"]/img");
            recipeDto.Image = new StringBuilder().Append(HEMMETS_ROOT).Append(recept_bild.Attributes["src"].Value).ToString();

            var directions = recipe_document.DocumentNode.SelectNodes("//div[@class=\"receptrightcol\"]/table/tr/td").Select(x => x.InnerText);
            var ingredients = recipe_document.DocumentNode.SelectNodes("//div[@class=\"receptleftcol\"]/table/tr").Descendants().Select(x => x.InnerText);

            recipeDto.Directions = directions.ToList();
            recipeDto.Ingredients = ingredients.ToList();

            return recipeDto;
        }
    }
}