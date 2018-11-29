using System.Collections.Generic;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Ramsey.NET.Models;
using Ramsey.NET.Services;

namespace Ramsey.NET.Controllers
{
    [Route("recipe")]
    public class RecipeController : Controller
    {
        private readonly RamseyContext _ramseyContext;

        public RecipeController(RamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("suggest")]
        [HttpPost]
        public IActionResult Suggest([FromBody]List<string> ingredients)
        {
            BackgroundJob.Enqueue<ICrawlerService>(x => x.UpdateIndexAsync());
            return StatusCode(200);
        }
    }
}