using System;
using System.Collections.Generic;
using System.Linq;
using Ramsey.NET.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ramsey.NET.Controllers.Interfaces;
using Ramsey.NET.Interfaces;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;

namespace Ramsey.NET.Controllers.V2
{
    [Route("v2/rating")]
    public class RatingControllerV2 : Controller, IRatingController
    {
        private readonly IRamseyContext _ramseyContext;

        public RatingControllerV2(IRamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("update")]
        public async Task<IActionResult> AddRatingAsync([FromBody]RatingDtoV2 ratingDto)
        {
            var rating = _ramseyContext.RecipeRatings.AddIfNotExists(new Models.RecipeRating
            {
                RecipeId = ratingDto.RecipeId,
                UserId = ratingDto.UserId,
                Score = ratingDto.Score
            }, x => x.UserId == ratingDto.UserId && x.RecipeId == ratingDto.RecipeId);

            await _ramseyContext.SaveChangesAsync();
            rating.Score = ratingDto.Score;

            await _ramseyContext.SaveChangesAsync();

            var recipe = _ramseyContext.Recipes.Find(ratingDto.RecipeId);
            recipe.Rating = _ramseyContext.RecipeRatings.Where(x => x.RecipeId == ratingDto.RecipeId).Average(x => x.Score);

            _ramseyContext.Recipes.Update(recipe);

            await _ramseyContext.SaveChangesAsync();
            return StatusCode(200);
        }
    }
}