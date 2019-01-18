using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Controllers.Interfaces
{
    public interface IIngredientController<T>
    {
        IActionResult Suggest(string search);
        IActionResult VerifyCollection([FromBody]List<T> ingredients);
    }
}
