﻿using Ramsey.NET.Extensions;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsey.NET.Shared.Interfaces;

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
            var recipe = _context.Recipes.Find(recipeMetaDto.RecipeID) ?? new RecipeMeta { RecipeId = recipeMetaDto.RecipeID };

            recipe.Image = recipeMetaDto.Image;
            recipe.Name = recipeMetaDto.Name;
            recipe.Owner = recipeMetaDto.Owner;
            recipe.OwnerLogo = recipeMetaDto.OwnerLogo;
            recipe.Source = recipeMetaDto.Source;

            if(!_context.Recipes.Any(x => x.RecipeId == recipe.RecipeId))
                _context.Recipes.Add(recipe);

            foreach (var i in recipeMetaDto.Ingredients)
            {
                var ingredientId = i;
                var ingredient = _context.Ingredients.SingleOrDefault(x => x.IngredientId == ingredientId);

                if (ingredient == null)
                {
                    _context.Ingredients.Add(new Ingredient { IngredientId = ingredientId });
                    _context.SaveChangesAsync();
                }

                var parts = _context.RecipeParts.Where(x => x.IngredientId.Equals(ingredientId) && x.RecipeId.Equals(recipeMetaDto.RecipeID)).ToList();
                var partDtos = recipeMetaDto.RecipeParts.Where(x => x.IngredientID.Equals(ingredientId)).ToList();

                if (parts.Count > 0) continue;
                foreach(var partDto in partDtos)
                {
                    var part = new RecipePart();
                    part.RecipeId = recipe.RecipeId;
                    part.IngredientId = ingredientId;
                    part.Unit = partDto.Unit;
                    part.Quantity = partDto.Quantity;

                    _context.RecipeParts.Add(part);
                }
            }

            return true;
        }

        public async Task<bool> SaveRecipeChangesAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
