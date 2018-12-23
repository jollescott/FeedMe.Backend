using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Controllers.Interfaces;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Controllers.V2
{
    [Route("v2/ingredient")]
    public class IngredientControllerV2 : Controller, IIngredientController
    {
        private readonly RamseyContext _ramseyContext;

        public IngredientControllerV2(RamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("suggest")]
        public IActionResult Suggest(string search)
        {
            var ingredientsDtos = new List<IngredientDtoV2>();

            var ingredients = _ramseyContext.Ingredients.Where(x => x.IngredientID.Contains(search)).Include(x => x.RecipeParts).ToList();
            ingredientsDtos = ingredients.Select(x => new IngredientDtoV2
            {
                RecipeParts = x.RecipeParts.Select(y => new RecipePartDtoV2
                {
                    IngredientID = y.IngredientId,
                    RecipeID = y.RecipeId,
                    Quantity = y.Quantity,
                    Unit = y.Unit
                }).ToList(),
                IngredientId = x.IngredientID
            }).ToList();

            return Json(ingredientsDtos);
        }
    }
}