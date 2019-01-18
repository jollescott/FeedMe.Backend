using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramsey.NET.Ingredients.Interfaces
{
    public interface IIngredientResolver
    {
        void Init(IList<string> removal, IDictionary<string, IList<string>> synonyms);
        Task<string> ResolveIngredientAsync(string ingredient);
    }
}
