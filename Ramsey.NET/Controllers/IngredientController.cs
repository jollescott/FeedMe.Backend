﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GusteauSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ramsey.NET.Controllers
{
    [Route("ingredient")]
    public class IngredientController : Controller
    {
        private readonly RamseyContext _ramseyContext;

        public IngredientController(RamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("suggest")]
        [HttpGet]
        public IActionResult Suggest(string search)
        {
            var ingredients = _ramseyContext.Ingredients.Where(x => x.Name.StartsWith(search)).Include(x => x.RecipeParts).ToList();
            return Json(ingredients);
        }
    }
}