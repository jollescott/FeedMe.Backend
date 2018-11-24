using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using GusteauSharp.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Ramsey.NET.Dto;
using Ramsey.NET.Interfaces;

namespace Ramsey.NET.Controllers
{
    [Route("recipe")]
    public class RecipeController : Controller
    {
        private readonly RamseyContext _ramseyContext;
        private readonly IRecipeScraper _recipeScraper;

        public RecipeController(RamseyContext ramseyContext, IRecipeScraper recipeScraper)
        {
            _ramseyContext = ramseyContext;
            _recipeScraper = recipeScraper;
        }

        [Route("suggest")]
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> SuggestAsync([FromBody]List<string> ingredients)
        {
            var recipes = await _recipeScraper.ScrapeRecipiesAsync(ingredients);
            return Json(recipes);
        }
    }
}