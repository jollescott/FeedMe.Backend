using Microsoft.AspNetCore.Mvc;
using Ramsey.NET.Controllers.Interfaces.V2;
using Ramsey.NET.Interfaces;
using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Controllers.V2
{
    [Route("meta")]
    public class MetaControllerV2 : Controller, IMetaController
    {
        private readonly IRamseyContext _ramseyContext;

        public MetaControllerV2(IRamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("recipeCount")]
        public IActionResult GetRecipeCount(RamseyLocale locale = RamseyLocale.Swedish)
        {
            var count = _ramseyContext.Recipes.Where(x => x.Locale == locale)
                .Count();

            return Json(count);
        }
    }
}
