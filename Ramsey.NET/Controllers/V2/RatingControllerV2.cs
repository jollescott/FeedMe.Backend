using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ramsey.NET.Controllers.Interfaces;

namespace Ramsey.NET.Controllers.V2
{
    [Route("v2/rating")]
    public class RatingControllerV2 : Controller, IRatingController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}