using Ramsey.NET.Models;
using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Interfaces
{
    public interface IRecipeManager
    {
        Task<bool> UpdateRecipeMetaAsync(IRamseyContext context,RecipeMetaDtoV2 recipe);
        Task<Dictionary<string,bool>> UpdateRecipeDatabaseAsync(IRamseyContext context,IList<RecipeMetaDtoV2> recipes);
    }
}
