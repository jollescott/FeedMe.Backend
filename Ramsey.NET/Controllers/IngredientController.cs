using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Controllers
{
    [Route("ingredient")]
    public class IngredientController : Controller
    {
        private readonly RamseyContext _ramseyContext;

        public IngredientController(RamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("suggest")]
        public IActionResult Suggest(string search)
        {
            var ingredientsDtos = new List<IngredientDto>();

            var ingredients = _ramseyContext.Ingredients.Where(x => x.IngredientID.Contains(search)).Include(x => x.RecipeParts).ToList();
            ingredientsDtos = ingredients.Select(x => new IngredientDto
            {
                RecipeParts = x.RecipeParts.Select(y => new RecipePartDto
                {
                    IngredientID = y.IngredientId,
                    RecipeID = y.RecipeId
                }).ToList(),
                IngredientId = x.IngredientID
            }).ToList();

            return Json(ingredientsDtos);
        }
    }
}