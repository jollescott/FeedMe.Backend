﻿using System;
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
using CurlThin.SafeHandles;
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
        private CURLcode _global;
        private SafeEasyHandle _easy;
        private readonly string HEMMETS_ROOT = "https://kokboken.ikv.uu.se/";

        public RecipeController(RamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;

            _global = CurlNative.Init();
            // curl_easy_init() to create easy handle.
            _easy = CurlNative.Easy.Init();

            CurlNative.Easy.SetOpt(_easy, CURLoption.CAINFO, CurlResources.CaBundlePath);
            CurlNative.Easy.SetOpt(_easy, CURLoption.COOKIESESSION, 1);
            CurlNative.Easy.SetOpt(_easy, CURLoption.USERAGENT, "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Ubuntu Chromium/32.0.1700.107 Chrome/32.0.1700.107 Safari/537.36");
        }

        protected override void Dispose(bool disposing)
        {
            _easy.Dispose();

            if (_global == CURLcode.OK)
            {
                CurlNative.Cleanup();
            }
            base.Dispose(disposing);
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
            var html = DoCurl("", HEMMETS_ROOT + "sok.php");

            var query = new StringBuilder();
            foreach (var ing in ingredients)
            {
                query = query.Append(ing).Append(' ');
            }

            var postData = $"search_text={query.ToString()}&dummy=&search_type=all&rec_cats\"%\"5B\"%\"5D=all&submit_search=S\"%\"F6k&recid=&offset=0&searchid=";

            var recipe_html = DoCurl(postData, HEMMETS_ROOT + "sok.php");

            if (recipe_html != string.Empty)
            {
                var home_document = new HtmlDocument();
                home_document.LoadHtml(recipe_html);

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

            var directions = recipe_document.DocumentNode.SelectNodes("//div[@class=\"receptrightcol\"]/table/tr")
                .Skip(1)
                .Select(x => WebUtility.HtmlDecode(x.InnerText?.Trim()))
                .Select(y => y.Replace("\n",""))
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

        private string DoCurl(string postData, string url)
        {
            string html = string.Empty;
            StringBuilder query = new StringBuilder();

            CurlNative.Easy.SetOpt(_easy, CURLoption.URL, url);

            // This one has to be called before setting COPYPOSTFIELDS.
            CurlNative.Easy.SetOpt(_easy, CURLoption.POSTFIELDSIZE, Encoding.ASCII.GetByteCount(postData));
            CurlNative.Easy.SetOpt(_easy, CURLoption.COPYPOSTFIELDS, postData);

            var stream = new MemoryStream();
            CurlNative.Easy.SetOpt(_easy, CURLoption.WRITEFUNCTION, (data, size, nmemb, user) =>
            {
                var length = (int)size * (int)nmemb;
                var buffer = new byte[length];
                Marshal.Copy(data, buffer, 0, length);
                stream.Write(buffer, 0, length);
                return (UIntPtr)length;
            });

            var result = CurlNative.Easy.Perform(_easy);
            html = Encoding.GetEncoding(1252).GetString(stream.ToArray());

            return html;
        }
    }
}