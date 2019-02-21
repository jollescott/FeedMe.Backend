using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Ramsey.NET.Extensions;
using Ramsey.NET.Controllers.Interfaces.V2;
using Ramsey.NET.Interfaces;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Enums;

namespace Ramsey.NET.Controllers.V2
{
    [Route("v2/recipe")]
    public class RecipeControllerV2 : Controller, IRecipeControllerV2<IngredientDtoV2, RecipeDtoV2>
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
            var ingredientIds = ingredients.Where(x => x.Role == IngredientRole.Include).Select(x => x.IngredientId);
            var excludeIds = ingredients.Where(x => x.Role == IngredientRole.Exclude).Select(x => x.IngredientId);

            var includeRecipes = _ramseyContext.Ingredients
                .Where(x => ingredientIds.Any(y => y == x.IngredientId))
                .SelectMany(x => x.RecipeParts).ToList()
                .Select(x => x.RecipeId)
                .Distinct();

            var excludeRecipes = _ramseyContext.Ingredients
                 .Where(x => excludeIds.Any(y => y == x.IngredientId))
                 .SelectMany(x => x.RecipeParts).ToList()
                 .Select(x => x.RecipeId)
                 .Distinct();           

            var recipeIds = includeRecipes.Where(x => excludeRecipes.All(y => x != y));

            var dtos = _ramseyContext.RecipeParts
                .AsNoTracking()
                .Include(x => x.Ingredient)
                .Where(x => recipeIds.Contains(x.RecipeId))
                .GroupBy(x => x.RecipeId)
                .Select(x => new RecipeMetaDtoV2
                {
                    RecipeID = x.Key,
                    Coverage = x.Where(y => ingredientIds.Contains(y.IngredientId)).DoubleCount() / x.Count()
                })
                .OrderByDescending(x => x.Coverage)
                .ThenBy(x => x.Name)
                .Skip(start)
                .Take(25)
                .ToList();

            foreach(var dto in dtos)
            {
                var recipe = _ramseyContext.Recipes.Find(dto.RecipeID);
                dto.Image = recipe.Image;
                dto.Name = recipe.Name;
                dto.Source = recipe.Source;
                dto.OwnerLogo = recipe.OwnerLogo;
                dto.Owner = recipe.Owner;
            }

            return Json(dtos);
        }

        [Route("retrieve")]
        public async Task<IActionResult> RetrieveAsync(string id)
        {
            var meta = await _ramseyContext.Recipes.AsNoTracking().Include(x => x.RecipeParts).SingleOrDefaultAsync(x => x.RecipeId == id);
            var recipe = await _crawlerService.ScrapeRecipeAsync(meta.Source, meta.Owner);

            return Json(recipe);
        }

        [Route("text")]
        [HttpPost]
        public IActionResult Text(string search, int start = 0, RamseyLocale locale = RamseyLocale.Swedish)
        {
            var recipes = _ramseyContext.Recipes
                .Where(x => EF.Functions.Like(x.Name, $"%{search}%"))
                .Where(x => x.Locale == locale)
                .OrderBy(x => x.Name)
                .Skip(start)
                .Take(25);

            return Json(recipes);
        }
    }
}