using Microsoft.AspNetCore.Mvc;
using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Controllers.Interfaces.V2
{
    public interface ITagsControllerV2
    {
        [Route("suggest")]
        [HttpPost]
        IActionResult Suggest(RamseyLocale locale = RamseyLocale.Swedish);

        [Route("list")]
        [HttpPost]
        IActionResult List(int tagid, int start = 0);
    }
}
