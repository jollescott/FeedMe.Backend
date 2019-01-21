﻿using System;
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
        public IActionResult Suggest([FromBody]List<IngredientDtoV2> ingredients)
        {
            var incIngredients = ingredients.Where(x => x.Role == IngredientRole.Include).ToList();
            var excIngredients = ingredients.Where(x => x.Role == IngredientRole.Exclude).ToList();
            
            var recipeIds = incIngredients.SelectMany(x => x.RecipeParts)
                .Select(x => x.RecipeID).Distinct().ToList();

            var recipes = recipeIds.Select(x => _ramseyContext.Recipes.Include(z => z.RecipeParts)
                .Single(y => y.RecipeId.Equals(x)))
                .Where(i => i.RecipeParts.All(j => excIngredients.All(k => j.IngredientId != k.IngredientId)));

            var dtos = recipes.Select(x => new RecipeMetaDtoV2
            {
                Image = x.Image,
                RecipeID = x.RecipeId,
                Source = x.Source,
                Name = x.Name,
                OwnerLogo = x.OwnerLogo,
                Owner = x.Owner,
                Ingredients = _ramseyContext.RecipeParts.Where(y => y.RecipeId == x.RecipeId)
                    .Include(j => j.Ingredient)
                    .Select(z => z.Ingredient.IngredientName),
                RecipeParts = x.RecipeParts.Select(y => new RecipePartDtoV2
                {
                    IngredientID = y.IngredientId,
                    Quantity = y.Quantity,
                    RecipeID = y.RecipeId,
                    Unit = y.Unit
                }),
                Coverage = (double)x.RecipeParts
                    .Select(y => y.IngredientId)
                    .Intersect(incIngredients.Select(y => y.IngredientId))
                    .Count() / x.RecipeParts.Count

            })
            .OrderByDescending(x => x.Coverage)
            .ToList();
            
            return Json(dtos);
        }

        [Route("retrieve")]
        public async Task<IActionResult> RetrieveAsync(string id)
        {
            var meta = await _ramseyContext.Recipes.Include(x => x.RecipeParts).SingleOrDefaultAsync(x => x.RecipeId == id);
            var recipe = await _crawlerService.ScrapeRecipeAsync(meta.Source, meta.Owner);

            return Json(recipe);
        }

        public IActionResult VerifyCollection([FromBody] List<IngredientDtoV2> recipes)
        {
            throw new NotImplementedException();
        }
    }
}