using System.Collections.Generic;
using System.Linq;
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

        [Route("suggest")]
        [HttpPost]
        public IActionResult Suggest([FromBody]List<IngredientDto> ingredients)
        {
            var recipeIds = ingredients.SelectMany(x => x.RecipeParts).Select(x => x.RecipeID).ToList();
            var recipes = recipeIds.Select(x => _ramseyContext.Recipes.Find(x)).ToList();

            return Json(recipes);
        }
    }
}