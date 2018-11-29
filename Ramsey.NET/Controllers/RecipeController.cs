using System.Collections.Generic;
using GusteauSharp.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async System.Threading.Tasks.Task<IActionResult> SuggestAsync([FromBody]List<string> ingredients)
        {
            return StatusCode(200);
        }
    }
}