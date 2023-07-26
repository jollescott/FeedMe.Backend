using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramsey.Core.Interfaces;
using Ramsey.NET.Controllers.Interfaces;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Enums;

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
        public IActionResult Suggest(string search, RamseyLocale locale = RamseyLocale.Swedish)
        {
            var ingredients = _ramseyContext.Ingredients
                .Where(x => EF.Functions.Like(x.IngredientName, $"%{search}%"))
                .OrderBy(x => x.IngredientName.Length)
                .Take(10);

            var ingredientsDtos = ingredients.Select(x => new IngredientDtoV2
            {
                IngredientId = x.IngredientId,
                IngredientName = x.IngredientName,
                Role = IngredientRole.Include
            });

            return Json(ingredientsDtos);
        }
    }
}