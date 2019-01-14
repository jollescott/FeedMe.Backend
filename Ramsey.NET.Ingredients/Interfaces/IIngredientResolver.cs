using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ramsey.NET.Ingredients.Interfaces
{
    public interface IIngredientResolver
    {
        Task<IIngredient> ResolveIngredientAsync(IIngredient ingredient);
    }
}
