using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Ramsey.NET.Controllers.Interfaces;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;

namespace Ramsey.NET.Controllers.V2
{
    [Route("v2/recipe")]
    public class RecipeControllerV2 : Controller, IRecipeController<IngredientDtoV2, RecipeDtoV2>
    {
        private readonly IRamseyContext _ramseyContext;
        private readonly ICrawlerService _crawlerService;

        public RecipeControllerV2(IRamseyContext ramseyContext, ICrawlerService crawlerService)
        {
            _ramseyContext = ramseyContext;
            _crawlerService = crawlerService;
        }

        [Route("reindex")]
        public IActionResult ReIndex()
        {
            //BackgroundJob.Enqueue<ICrawlerService>(x => x.UpdateIndexAsync());
            return StatusCode(200);
        }

        [Route("patch")]
        public IActionResult Patch()
        {
            //BackgroundJob.Enqueue<IPatcherService>(x => x.PatchIngredientsAsync());
            return StatusCode(200);
        }

        [Route("suggest")]
        [HttpPost]
        public IActionResult Suggest([FromBody]List<IngredientDtoV2> ingredients, int start = 0)
        {
            var incIngredients = ingredients.Where(x => x.Role == IngredientRole.Include).Select(x => x.IngredientId);
            var excIngredients = ingredients.Where(x => x.Role == IngredientRole.Exclude).Select(x => x.IngredientId);

            var recipeIds = _ramseyContext.RecipeParts
                .Where(x => incIngredients.Any(y => y == x.IngredientId))
                .Select(x => x.RecipeId)
                .Distinct();

            /*
            var foundRecipes = recipeIds.Select(x => _ramseyContext.Recipes.Include(z => z.RecipeParts)
                .AsNoTracking()
                .Single(y => y.RecipeId.Equals(x)))
                .Where(i => i.RecipeParts.All(j => excIngredients.All(k => j.IngredientId != k.IngredientId)));
            */

            var foundRecipes = _ramseyContext.Recipes
                .Where(x => recipeIds.Any(y => y == x.RecipeId))
                .Where(x => x.RecipeParts.All(y => excIngredients.All(z => y.IngredientId != z)))
                .Include(x => x.RecipeParts)
                .ThenInclude(x => x.Ingredient);

            //Calc coverage first
            var dtos = foundRecipes.Select(x => new RecipeMetaDtoV2
            {
                RecipeID = x.RecipeId,
                Coverage = (double)x.RecipeParts
                    .Select(y => y.IngredientId)
                    .Intersect(incIngredients.Select(y => y))
                    .Count() / x.RecipeParts.Count,

                Ingredients = x.RecipeParts.Select(y => y.Ingredient.IngredientName),

                RecipeParts = x.RecipeParts.Select(y => new RecipePartDtoV2
                {
                    IngredientID = y.IngredientId,
                    IngredientName = y.Ingredient.IngredientName,
                    Quantity = y.Quantity,
                    RecipeID = y.RecipeId,
                    Unit = y.Unit
                }),

                Image = x.Image,
                Source = x.Source,
                Name = x.Name,
                OwnerLogo = x.OwnerLogo,
                Owner = x.Owner
            })
            .OrderByDescending(x => x.Coverage)
            .OrderByDescending(x => x.Name)
            .Skip(start)
            .Take(start + 25);
            
            return Json(dtos);
        }

        [Route("retrieve")]
        public async Task<IActionResult> RetrieveAsync(string id)
        {
            var meta = await _ramseyContext.Recipes.AsNoTracking().Include(x => x.RecipeParts).SingleOrDefaultAsync(x => x.RecipeId == id);
            var recipe = await _crawlerService.ScrapeRecipeAsync(meta.Source, meta.Owner);

            return Json(recipe);
        }

        public IActionResult VerifyCollection([FromBody] List<IngredientDtoV2> recipes)
        {
            throw new NotImplementedException();
        }
    }
}