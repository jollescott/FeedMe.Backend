﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using CurlThin;
using CurlThin.Enums;
using CurlThin.Helpers;
using CurlThin.Native;
using CurlThin.SafeHandles;
using GusteauSharp.Dto;
using GusteauSharp.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
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
            CurlNative.Easy.SetOpt(_easy, CURLoption.USERAGENT, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0");

            var headers = CurlNative.Slist.Append(SafeSlistHandle.Null, "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            // Add one more value to existing HTTP header list.
            CurlNative.Slist.Append(headers, "Accept-Language: sv-SE,sv;q=0.8,en-US;q=0.5,en;q=0.3");
            CurlNative.Slist.Append(headers, "Referer: https://kokboken.ikv.uu.se/sok.php");
            CurlNative.Slist.Append(headers, "Content-Type: application/x-www-form-urlencoded");
            CurlNative.Slist.Append(headers, "Connection: keep-alive");
            CurlNative.Slist.Append(headers, "Upgrade-Insecure-Requests: 1");

            // Configure libcurl easy handle to send HTTP headers we configured.
            CurlNative.Easy.SetOpt(_easy, CURLoption.HTTPHEADER, headers.DangerousGetHandle());
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

        [Route("suggest")]
        [HttpPost]
        public IActionResult Suggest([FromBody]List<string> ingredients)
        {
            var postData = "search_text=tomat&dummy=&search_type=all&rec_cats\"%\"5B\"%\"5D=all&submit_search=S\"%\"F6k&recid=&offset=0&searchid=";

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
            CurlNative.Easy.SetOpt(_easy, CURLoption.POSTFIELDS, postData);
            CurlNative.Easy.SetOpt(_easy, CURLoption.POST, 1);
            CurlNative.Easy.SetOpt(_easy, CURLoption.SSL_VERIFYHOST, 0);
            CurlNative.Easy.SetOpt(_easy, CURLoption.SSL_VERIFYPEER, 0);

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