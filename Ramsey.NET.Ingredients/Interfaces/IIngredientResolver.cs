using System.Threading.Tasks;

namespace Ramsey.NET.Ingredients.Interfaces
{
    public interface IIngredientResolver
    {
        Task<string> ResolveIngredientAsync(string ingredient);
    }
}
