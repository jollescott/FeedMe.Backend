using Ramsey.Shared.Dto.V2;
using System.Threading.Tasks;

namespace Ramsey.NET.Shared.Interfaces
{
    public interface IRecipeManager
    {
        Task<bool> UpdateRecipeMetaAsync(RecipeMetaDtoV2 recipe);
        Task<bool> SaveRecipeChangesAsync();

        Task ReportFailedRecipeAsync(string url, string message);
    }
}
