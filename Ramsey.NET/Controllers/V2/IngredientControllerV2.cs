using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Controllers.Interfaces;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;

namespace Ramsey.NET.Controllers.V2
{
    [Route("v2/ingredient")]
    public class IngredientControllerV2 : Controller, IIngredientController<IngredientDtoV2>
    {
        private readonly IRamseyContext _ramseyContext;

        public IngredientControllerV2(IRamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("suggest")]
        public IActionResult Suggest(string search)
        {
            var ingredientsDtos = new List<IngredientDtoV2>();

            var ingredients = _ramseyContext.Ingredients.Where(x => x.IngredientName.Contains(search))
                .OrderBy(x => x.IngredientName.Length)
                .Take(25)
                .Include(x => x.RecipeParts)
                .ToList();

            ingredientsDtos = ingredients.Select(x => new IngredientDtoV2
            {
                RecipeParts = x.RecipeParts.Select(y => new RecipePartDtoV2
                {
                    IngredientID = y.IngredientId,
                    RecipeID = y.RecipeId,
                    Quantity = y.Quantity,
                    Unit = y.Unit,
                }).ToList(),
                IngredientId = x.IngredientId,
                IngredientName = x.IngredientName,
                Role = IngredientRole.Include
            }).ToList();

            return Json(ingredientsDtos);
        }

        public IActionResult VerifyCollection([FromBody] List<IngredientDtoV2> ingredients)
        {
            throw new NotImplementedException();
        }
    }
}