using System.Collections.Generic;
using System.Threading.Tasks;
using Ramsey.NET.Shared.Interfaces;
using Ramsey.Shared.Dto.V2;

namespace Ramsey.NET.Ingredients.Interfaces
{
    public interface IIngredientInspector<T>
    {
        void InspectIngredient(IList<T> ingredients);
    }
}