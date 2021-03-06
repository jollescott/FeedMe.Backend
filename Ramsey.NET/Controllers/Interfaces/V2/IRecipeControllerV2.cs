﻿using Microsoft.AspNetCore.Mvc;
using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Controllers.Interfaces.V2
{
    public interface IRecipeControllerV2<TIngredient, TRecipe> : IRecipeController<TIngredient, TRecipe>
    {
        IActionResult Text(string search, int start = 0, RamseyLocale locale = RamseyLocale.Swedish);
    }
}
