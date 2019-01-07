using Ramsey.NET.Extensions;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;
using System.Text.RegularExpressions;

namespace Ramsey.NET.Implementations
{
    public class SqlRecipeManager : IRecipeManager
    {
        private  readonly IRamseyContext _context;

        public SqlRecipeManager(IRamseyContext context)
        {
            _context = context;
        }
        public async Task<bool> UpdateRecipeMetaAsync(RecipeMetaDtoV2 recipeMetaDto)
        {
            var recipe = _context.Recipes.AddIfNotExists(new RecipeMeta
            {
                RecipeId = recipeMetaDto.RecipeID,
            }, x => x.RecipeId == recipeMetaDto.RecipeID);

            await SaveRecipeChangesAsync();

            //Basic Properties
            recipe.Image = recipeMetaDto.Image;
            recipe.Name = recipeMetaDto.Name;
            recipe.Owner = recipeMetaDto.Owner;
            recipe.OwnerLogo = recipeMetaDto.OwnerLogo;
            recipe.Rating = 0;
            recipe.Source = recipeMetaDto.Source;

            //Ingredients
            foreach(var partDto in recipeMetaDto.RecipeParts)
            {
                string ingredientId = partDto.IngredientID.FormatIngredientName();
                string recipeId = recipeMetaDto.RecipeID;

                if (ingredientId == null || recipeId == null ||
                    ingredientId == string.Empty || recipeId == string.Empty || 
                    ingredientId.Contains("och"))
                    continue;

                var ingredient = _context.Ingredients.AddIfNotExists(new Ingredient
                {
                    IngredientId = ingredientId
                }, x => x.IngredientId == ingredientId);

                await SaveRecipeChangesAsync();

                var part = _context.RecipeParts.AddIfNotExists(new RecipePart
                {
                    IngredientId = ingredientId,
                    RecipeId = recipeId
                }, x => x.RecipeId == recipeId && x.IngredientId == ingredientId);

                await SaveRecipeChangesAsync();

                part.Ingredient = ingredient;
                part.Recipe = recipe;

                part.Quantity = partDto.Quantity;
                part.Unit = partDto.Unit;

                _context.RecipeParts.Update(part);

                await SaveRecipeChangesAsync();
                
                recipe.RecipeParts.Add(part);
            }

            _context.Recipes.Update(recipe);
            await SaveRecipeChangesAsync();

            return true;
        }

        public async Task<bool> SaveRecipeChangesAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
