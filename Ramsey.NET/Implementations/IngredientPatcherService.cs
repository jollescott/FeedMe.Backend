using Hangfire;
using Ramsey.Core;
using Ramsey.NET.Extensions;
using Ramsey.NET.Interfaces;
using Ramsey.NET.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Implementations
{
    public class IngredientPatcherService : IPatcherService
    {
        private readonly IRamseyContext _ramseyContext;
        private readonly IIllegalRemover _illegalRemover;

        public IngredientPatcherService(IRamseyContext ramseyContext, IIllegalRemover illegalRemover)
        {
            _ramseyContext = ramseyContext;
            _illegalRemover = illegalRemover;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task PatchIngredientsAsync()
        {
            var toModify = new List<ModifyIngredient>();
            
            foreach(var ingredient in _ramseyContext.Ingredients)
            {
                var oldName = ingredient.IngredientName;
                var newName = _illegalRemover.RemoveIllegals(ingredient.IngredientName);

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

                var toChange = _ramseyContext.RecipeParts.Where(x => x.IngredientId == modify.OldIngredient.IngredientId).ToList();

                foreach(var part in toChange)
                {
                    part.Ingredient = newIngredient;
                    _ramseyContext.RecipeParts.Update(part);

                    await _ramseyContext.SaveChangesAsync();
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
