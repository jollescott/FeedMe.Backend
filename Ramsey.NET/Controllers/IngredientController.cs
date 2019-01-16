using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Controllers.Interfaces;
using Ramsey.NET.Interfaces;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Controllers
{
    [Route("ingredient")]
    public class IngredientController : Controller, IIngredientController
    {
        private readonly IRamseyContext _ramseyContext;

        public IngredientController(IRamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("suggest")]
        public IActionResult Suggest(string search)
        {
            var ingredientsDtos = new List<IngredientDto>();

            var ingredients = _ramseyContext.Ingredients.Where(x => x.IngredientName.Contains(search)).Include(x => x.RecipeParts).ToList();
            ingredientsDtos = ingredients.Select(x => new IngredientDto
            {
                RecipeParts = x.RecipeParts.Select(y => new RecipePartDto
                {
                    IngredientID = y.IngredientId,
                    RecipeID = y.RecipeId
                }).ToList(),
                IngredientId = x.IngredientName
            }).ToList();

            return Json(ingredientsDtos);
        }
    }
}