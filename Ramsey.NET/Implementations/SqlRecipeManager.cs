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
        public async Task<Dictionary<string, bool>> UpdateRecipeDatabaseAsync(RamseyContext context,List<RecipeMetaDtoV2> recipes)
        {
            var results = new Dictionary<string, bool>();

            foreach (var recipeMetaDto in recipes)
            {
                var result = await UpdateRecipeMeta(context, recipeMetaDto);
            }

            await context.SaveChangesAsync();
            return results;
        }

        public Task<bool> UpdateRecipeMeta(RamseyContext context, RecipeMetaDtoV2 recipeMetaDto)
        {
            var recipe = context.Recipes.Find(recipeMetaDto.RecipeID);
            if (recipe == null) recipe = new RecipeMeta { RecipeId = recipeMetaDto.RecipeID };

            recipe.Image = recipeMetaDto.Image;
            recipe.Name = recipeMetaDto.Name;
            recipe.Owner = recipeMetaDto.Owner;
            recipe.OwnerLogo = recipeMetaDto.OwnerLogo;
            recipe.Source = recipeMetaDto.Source;

            context.AddOrUpdate(recipe);

            foreach (var i in recipeMetaDto.Ingredients)
            {
                var ingredient_id = i;
                var ingredient = context.Ingredients.Where(x => x != null && x.IngredientID == ingredient_id).SingleOrDefault();

                if (ingredient == null)
                {
                    context.Ingredients.Add(new Ingredient { IngredientID = ingredient_id });
                    context.SaveChanges();
                }

                var parts = context.RecipeParts.Where(x => x.IngredientId.Equals(ingredient_id) && x.RecipeId.Equals(recipeMetaDto.RecipeID)).ToList();
                var partDtos = recipeMetaDto.RecipeParts.Where(x => x.IngredientID.Equals(ingredient_id)).ToList();
                
                if (parts == null || parts.Count <= 0)
                {
                    foreach(var partDto in partDtos)
                    {
                        var part = new RecipePart();
                        part.RecipeId = recipe.RecipeId;
                        part.IngredientId = ingredient_id;
                        part.Unit = partDto.Unit;
                        part.Quantity = partDto.Quantity;

                        context.RecipeParts.Add(part);
                        context.SaveChanges();
                    }
                }
            }

            return Task.FromResult(true);
        }
    }
}
