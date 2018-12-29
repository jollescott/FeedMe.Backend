using Ramsey.Shared.Dto;
using System.Threading.Tasks;

namespace Ramsey.NET.Shared.Interfaces
{
    public interface IRecipeManager
    {
        Task<bool> UpdateRecipeMetaAsync(RecipeMetaDtoV2 recipe);
        Task<bool> SaveRecipeChangesAsync();
    }
}
