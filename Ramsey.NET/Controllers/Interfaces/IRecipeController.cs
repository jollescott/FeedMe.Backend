using Microsoft.AspNetCore.Mvc;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Controllers.Interfaces
{
    public interface IRecipeController<TIngredient, TRecipe>
    {
        IActionResult Suggest([FromBody]List<TIngredient> ingredients, int start = 0);
        Task<IActionResult> RetrieveAsync(string id);
    }
}
