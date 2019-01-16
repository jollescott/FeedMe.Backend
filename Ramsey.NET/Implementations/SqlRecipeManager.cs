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
using Ramsey.NET.Ingredients.Interfaces;

namespace Ramsey.NET.Implementations
{
    public class SqlRecipeManager : IRecipeManager
    {
        private readonly IRamseyContext _context;
        private readonly IIngredientResolver _ingredientResolver;

        public SqlRecipeManager(IRamseyContext context, IIngredientResolver ingredientResolver)
        {
            _context = context;
            _ingredientResolver = ingredientResolver;
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
                string ingredientName = await _ingredientResolver.ResolveIngredientAsync(partDto.IngredientName);
                string recipeId = recipeMetaDto.RecipeID;

                /*
                if (ingredientName == null || recipeId == null ||
                    ingredientName == string.Empty || recipeId == string.Empty || 
                    ingredientName.Contains("och"))
                    continue;
                    */

                var ingredient = _context.Ingredients.AddIfNotExists(new Ingredient
                {
                    IngredientName = ingredientName
                }, x => x.IngredientName == ingredientName);

                await SaveRecipeChangesAsync();

                var part = _context.RecipeParts.AddIfNotExists(new RecipePart
                {
                    RecipeId = recipeId,
                    IngredientId = ingredient.IngredientId
                }, x => x.RecipeId == recipeId && x.Ingredient.IngredientName == ingredientName);

                await SaveRecipeChangesAsync();

                part.IngredientId = ingredient.IngredientId;
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
