using System.Threading.Tasks;

namespace Ramsey.NET.Ingredients.Interfaces
{
    public abstract class ABasicIngredientResolver : IIngredientResolver
    {
        public abstract Task<string> ResolveIngredientAsync(string ingredient);

        public abstract Task<string> ApplyRegexesAsync(string ingredient);
        public abstract Task<string> LinkSynonymsAsync(string ingredient);
        public abstract Task<string> RemoveIllegalsAsync(string ingredient);
    }
}
