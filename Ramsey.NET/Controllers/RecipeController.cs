using System.Collections.Generic;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Ramsey.NET.Models;
using Ramsey.NET.Services;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Controllers
{
    [Route("recipe")]
    public class RecipeController : Controller
    {
        private readonly RamseyContext _ramseyContext;

        public RecipeController(RamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("index")]
        public IActionResult Index()
        {
            BackgroundJob.Enqueue<ICrawlerService>(x => x.UpdateIndexAsync());
            return StatusCode(200);
        }

        [Route("suggest")]
        [HttpPost]
        public IActionResult Suggest([FromBody]List<IngredientDto> ingredients)
        {
            var recipes = new List<RecipeDto>();
            return Json(recipes);
        }
    }
}