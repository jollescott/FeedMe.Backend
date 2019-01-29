using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramsey.NET.Ingredients.Interfaces
{
    public abstract class ABasicIngredientResolver : IIngredientResolver
    {
        public abstract void Init(IList<string> removal, IDictionary<string, IList<string>> synonyms);

        public abstract Task<string> ResolveIngredientAsync(string ingredient);

        public abstract Task<string> ApplyRegexesAsync(string ingredient);
        public abstract Task<string> LinkSynonymsAsync(string ingredient);
        public abstract void RemoveIllegals(ref string name);
    }
}
