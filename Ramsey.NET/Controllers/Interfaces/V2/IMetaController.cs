using Microsoft.AspNetCore.Mvc;
using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Controllers.Interfaces.V2
{
    public interface IMetaController
    {
        IActionResult GetRecipeCount(RamseyLocale locale = RamseyLocale.Swedish);
    }
}
