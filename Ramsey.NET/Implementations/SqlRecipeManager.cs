using Ramsey.NET.Extensions;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Implementations
{
    public class SqlRecipeManager : IRecipeManager
    {
        public async Task<Dictionary<string, bool>> UpdateRecipeDatabaseAsync(IRamseyContext context,List<RecipeMetaDtoV2> recipes)
        {
            var results = new Dictionary<string, bool>();

            foreach (var recipeMetaDto in recipes)
            {
                var result = await UpdateRecipeMetaAsync(context, recipeMetaDto);
            }

            await context.SaveChangesAsync();
            return results;
        }

        public async Task<bool> UpdateRecipeMetaAsync(IRamseyContext context, RecipeMetaDtoV2 recipeMetaDto)
        {
            var recipe = context.Recipes.Find(recipeMetaDto.RecipeID);
            if (recipe == null) recipe = new RecipeMeta { RecipeId = recipeMetaDto.RecipeID };

            recipe.Image = recipeMetaDto.Image;
            recipe.Name = recipeMetaDto.Name;
            recipe.Owner = recipeMetaDto.Owner;
            recipe.OwnerLogo = recipeMetaDto.OwnerLogo;
            recipe.Source = recipeMetaDto.Source;

            if(!context.Recipes.Any(x => x.RecipeId == recipe.RecipeId))
                context.Recipes.Add(recipe);

            foreach (var i in recipeMetaDto.Ingredients)
            {
                var ingredientId = i;
                var ingredient = context.Ingredients.SingleOrDefault(x => x != null && x.IngredientID == ingredientId);

                if (ingredient == null)
                {
                    context.Ingredients.Add(new Ingredient { IngredientID = ingredientId });
                }

                var parts = context.RecipeParts.Where(x => x.IngredientId.Equals(ingredientId) && x.RecipeId.Equals(recipeMetaDto.RecipeID)).ToList();
                var partDtos = recipeMetaDto.RecipeParts.Where(x => x.IngredientID.Equals(ingredientId)).ToList();
                
                if (parts.Count <= 0)
                {
                    foreach(var partDto in partDtos)
                    {
                        var part = new RecipePart();
                        part.RecipeId = recipe.RecipeId;
                        part.IngredientId = ingredientId;
                        part.Unit = partDto.Unit;
                        part.Quantity = partDto.Quantity;

                        context.RecipeParts.Add(part);
                    }
                }
            }

            return true;
        }
    }
}
