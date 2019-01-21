﻿using Microsoft.AspNetCore.Mvc;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Controllers.Interfaces
{
    public interface IRecipeController<TIngredient, TRecipe>
    {
        [Route("reindex")]
        IActionResult ReIndex();

        [Route("suggest")]
        [HttpPost]
        IActionResult Suggest([FromBody]List<TIngredient> ingredients);

        IActionResult VerifyCollection([FromBody]List<TIngredient> recipes);

        [Route("retrieve")]
        Task<IActionResult> RetrieveAsync(string id);
    }
}
