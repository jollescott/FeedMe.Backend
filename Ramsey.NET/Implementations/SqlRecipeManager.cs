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
        public async Task<Dictionary<string, bool>> UpdateRecipeDatabaseAsync(RamseyContext context,List<RecipeMetaDto> recipes)
        {
            var results = new Dictionary<string, bool>();

            foreach (var recipeMetaDto in recipes)
            {
                var result = await UpdateRecipeMeta(context, recipeMetaDto);
            }

            await context.SaveChangesAsync();
            return results;
        }

        public Task<bool> UpdateRecipeMeta(RamseyContext context, RecipeMetaDto recipeMetaDto)
        {
            List<Ingredient> ings = new List<Ingredient>();

            foreach (var i in recipeMetaDto.Ingredients)
            {
                Ingredient ingredient = context.Ingredients.Find(i.ToLower());

                if (ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        IngredientID = i.ToLower()
                    };

                    context.Ingredients.Add(ingredient);
                }

                ings.Add(ingredient);
            }

            RecipeMeta recipe = context.Recipes.Find(recipeMetaDto.RecipeID);

            if (recipe == null)
            {
                recipe = new RecipeMeta
                {
                    Image = recipeMetaDto.Image,
                    Name = recipeMetaDto.Name,
                    Owner = recipeMetaDto.Owner,
                    Source = recipeMetaDto.Source,
                    RecipeId = recipeMetaDto.RecipeID
                };

                context.Recipes.Add(recipe);

                var recipeParts = ings.Select(x => new RecipePart
                {
                    IngredientId = x.IngredientID,
                    RecipeId = recipe.RecipeId
                });

                context.RecipeParts.AddRange(recipeParts);
            }

            return Task.FromResult(true);
        }
    }
}
