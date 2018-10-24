using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GusteauSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet]
        public async Task<IActionResult> SuggestAsync(string search)
        {
            var ingredients = await _ramseyContext.Ingredients.Where(x => x.Name.StartsWith(search)).ToListAsync();
            return Json(ingredients);
        }
    }
}