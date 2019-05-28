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
        IActionResult Suggest(RamseyLocale locale = RamseyLocale.Swedish);

        IActionResult List(int tagid, int start = 0);
    }
}
