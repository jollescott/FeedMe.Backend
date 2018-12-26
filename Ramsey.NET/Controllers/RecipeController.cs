using System.Collections.Generic;
using System.Linq;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Controllers.Interfaces;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Controllers
{
    [Route("recipe")]
    public class RecipeController : Controller, IRecipeController<IngredientDto>
    {
        private readonly IRamseyContext _ramseyContext;
        private readonly ICrawlerService _crawlerService;

        public RecipeController(IRamseyContext ramseyContext, ICrawlerService crawlerService)
        {
            _ramseyContext = ramseyContext;
            _crawlerService = crawlerService;
        }

        [Route("reindex")]
        public IActionResult ReIndex()
        {
            BackgroundJob.Enqueue<ICrawlerService>(x => x.UpdateIndexAsync());
            return StatusCode(200);
        }

        [Route("suggest")]
        [HttpPost]
        public IActionResult Suggest([FromBody]List<IngredientDto> ingredients)
        {
            var recipeIds = ingredients.SelectMany(x => x.RecipeParts).Select(x => x.RecipeID).ToList();
            var recipes = recipeIds.Select(x => _ramseyContext.Recipes.Find(x)).ToList();

            return Json(recipes);
        }

        [Route("retrieve")]
        public async System.Threading.Tasks.Task<IActionResult> RetrieveAsync(string id)
        {
            var meta = await _ramseyContext.Recipes.Include(x => x.RecipeParts).SingleOrDefaultAsync(x => x.RecipeId == id);
            var recipeV2 = await _crawlerService.ScrapeRecipeAsync(meta.Source, meta.Owner);

            var compRecipe = new RecipeDto
            {
                RecipeID = recipeV2.RecipeID,
                Desc = recipeV2.Desc,
                Image = recipeV2.Image,
                Directions = recipeV2.Directions.ToList(),
                Ingredients = recipeV2.Ingredients.ToList(),
                Source = recipeV2.Source,
                Name = recipeV2.Name,
                Owner = recipeV2.Owner,
                OwnerLogo = recipeV2.OwnerLogo
            };

            return Json(compRecipe);
        }
    }
}