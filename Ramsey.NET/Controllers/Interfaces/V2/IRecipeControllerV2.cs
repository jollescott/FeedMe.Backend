using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Controllers.Interfaces.V2
{
    public interface IRecipeControllerV2<TIngredient, TRecipe> : IRecipeController<TIngredient, TRecipe>
    {
        [Route("text")]
        [HttpPost]
        IActionResult Text(string search, int start = 0);
    }
}
