﻿using Ramsey.NET.Extensions;
using Ramsey.NET.Ingredients.Interfaces;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Implementations
{
    public class IngredientPatcherService : IPatcherService
    {
        private readonly IIngredientResolver _ingredientResolver;
        private readonly IRamseyContext _ramseyContext;

        public IngredientPatcherService(IIngredientResolver ingredientResolver, IRamseyContext ramseyContext)
        {
            _ingredientResolver = ingredientResolver;
            _ramseyContext = ramseyContext;
        }

        public async Task PatchIngredientsAsync()
        {
            var toModify = new List<ModifyIngredient>();

            foreach(var ingredient in _ramseyContext.Ingredients)
            {
                var oldName = ingredient.IngredientName;
                var newName = await _ingredientResolver.ResolveIngredientAsync(ingredient.IngredientName);

                if(oldName != newName)
                {
                    toModify.Add(new ModifyIngredient
                    {
                        OldIngredient = ingredient,
                        NewName = newName
                    });
                }
            }

            foreach(var modify in toModify)
            {
                var newIngredient = _ramseyContext.Ingredients.AddIfNotExists(new Ingredient
                {
                    IngredientName = modify.NewName
                }, x => x.IngredientName == modify.NewName);

                await _ramseyContext.SaveChangesAsync();

                var toChange = _ramseyContext.RecipeParts.Where(x => x.IngredientId == modify.OldIngredient.IngredientId);

                foreach(var part in toChange)
                {
                    part.IngredientId = newIngredient.IngredientId;
                    _ramseyContext.RecipeParts.Update(part);
                }

                _ramseyContext.Ingredients.Remove(modify.OldIngredient);

                await _ramseyContext.SaveChangesAsync();
            }
        }
    }

    internal class ModifyIngredient
    {
        public Ingredient OldIngredient { get; set; }
        public string NewName { get; set; }
    }
}