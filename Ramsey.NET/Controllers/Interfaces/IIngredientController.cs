using Microsoft.AspNetCore.Mvc;
using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Controllers.Interfaces
{
    public interface IIngredientController<T>
    {
        IActionResult Suggest(string search, RamseyLocale locale = RamseyLocale.Swedish);
    }
}
