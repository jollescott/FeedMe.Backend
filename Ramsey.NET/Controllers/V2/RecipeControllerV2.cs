using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Ramsey.NET.Controllers.Interfaces;
using Ramsey.NET.Controllers.Interfaces.V2;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;

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

            var recipeParts = _ramseyContext.RecipeParts
                .Where(x => recipeIds.Any(y => y == x.RecipeId))
                .ToList();

            var dtos = new List<RecipeMetaDtoV2>();
            Parallel.ForEach(recipeIds, (id) =>
            {
                var dto = new RecipeMetaDtoV2 { RecipeID = id };

                var ings = recipeParts
                    .Where(x => x.RecipeId == id)
                    .Select(x => x.IngredientId)
                    .Distinct();

                var matching = ings
                    .Intersect(ingredientIds)
                    .ToList();

                dto.Coverage = (double)matching.Count() / ings.Count();

                dtos.Add(dto);
            });

            dtos = dtos
                .OrderByDescending(x => x.Coverage)
                .ThenBy(x => x.Name)
                .Skip(start)
                .Take(25)
                .ToList();

            var keptIds = dtos.Select(x => x.RecipeID);

            var recipes = _ramseyContext.Recipes
                .Include(x => x.RecipeParts)
                .ThenInclude(x => x.Ingredient)
                .Where(x => keptIds.Any(y => x.RecipeId == y))
                .ToList();

            Parallel.ForEach(dtos, (dto) =>
            {
                var recipe = recipes.Single(x => x.RecipeId == dto.RecipeID);

                dto.Name = recipe.Name;
                dto.Image = recipe.Image;
                dto.Source = recipe.Source;
                dto.Ingredients = recipe.RecipeParts.Select(x => x.Ingredient.IngredientName);
                dto.OwnerLogo = recipe.OwnerLogo;
                dto.Owner = recipe.Owner;
                dto.RecipeParts = recipe.RecipeParts.Select(x => new RecipePartDtoV2
                {
                    IngredientID = x.IngredientId,
                    IngredientName = x.Ingredient.IngredientName,
                    Quantity = x.Quantity,
                    RecipeID = x.RecipeId,
                    Unit = x.Unit
                });
            });

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
        public IActionResult Text(string search, int start = 0)
        {
            var recipes = _ramseyContext.Recipes
                .Where(x => EF.Functions.Like(x.Name, $"%{search}%"))
                .OrderBy(x => x.Name)
                .Skip(start)
                .Take(25);

            return Json(recipes);
        }
    }
}