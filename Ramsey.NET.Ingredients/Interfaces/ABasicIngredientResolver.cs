using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ramsey.Shared.Interfaces;

namespace Ramsey.NET.Ingredients.Interfaces
{
    public abstract class ABasicIngredientResolver : IIngredientResolver
    {
        public abstract Task LoadResourcesAsync();

        public abstract Task<IIngredient> ResolveIngredientAsync(IIngredient ingredient);
    }
}
