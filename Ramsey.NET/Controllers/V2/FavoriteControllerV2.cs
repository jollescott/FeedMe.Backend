using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Controllers.Interfaces;
using Ramsey.NET.Interfaces;
using Ramsey.Shared.Dto;

namespace Ramsey.NET.Controllers.V2
{
    [Route("v2/favorites")]
    public class FavoriteControllerV2 : Controller, IFavoriteController
    {
        private readonly IRamseyContext _ramseyContext;

        public FavoriteControllerV2(IRamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        [Route("add")]
        public async Task<IActionResult> AddFavoriteAsync([FromBody]FavoriteDtoV2 favoriteDto)
        {
            if(_ramseyContext.RecipeFavorites.Where(x => x.UserId == favoriteDto.UserId)
                .All(x => x.RecipeId != favoriteDto.RecipeId))
            {
                _ramseyContext.RecipeFavorites.Add(new Models.RecipeFavorite
                {
                    UserId = favoriteDto.UserId,
                    RecipeId = favoriteDto.RecipeId
                });
            }

            await _ramseyContext.SaveChangesAsync();
            return StatusCode(200);
        }

        [Route("delete")]
        public async Task<IActionResult> DeleteFavoriteAsync([FromBody]FavoriteDtoV2 favoriteDto)
        {
            var favorite = _ramseyContext.RecipeFavorites.SingleOrDefault(x => x.UserId == favoriteDto.UserId && x.RecipeId == favoriteDto.RecipeId);

            if(favoriteDto != null)
            {
                _ramseyContext.RecipeFavorites.Remove(favorite);
                await _ramseyContext.SaveChangesAsync();
            }

            return StatusCode(200);
        }

        [Route("list")]
        public async Task<IActionResult> GetListAsync([FromBody]UserDto userDto)
        {
            var favorites = _ramseyContext.RecipeFavorites.Where(x => x.UserId == userDto.UserId);

            List<RecipeMetaDtoV2> recipes = new List<RecipeMetaDtoV2>();
            await favorites.ForEachAsync(x =>
            {
                var recipe = new RecipeMetaDtoV2
                {
                    Coverage = 100,
                    Image = x.Recipe.Image,
                    Source = x.Recipe.Source,
                    Ingredients = x.Recipe.RecipeParts.Select(y => y.IngredientId),
                    RecipeID = x.RecipeId,
                    Name = x.Recipe.Name,
                    Owner = x.Recipe.Owner,
                    OwnerLogo = x.Recipe.OwnerLogo,
                    RecipeParts = x.Recipe.RecipeParts.Select(y => new RecipePartDtoV2
                    {
                        IngredientID = y.IngredientId,
                        Quantity = y.Quantity,
                        RecipeID = y.RecipeId,
                        Unit = y.Unit
                    }),
                };

                recipes.Add(recipe);
            });

            return Json(recipes);
        }
    }
}
