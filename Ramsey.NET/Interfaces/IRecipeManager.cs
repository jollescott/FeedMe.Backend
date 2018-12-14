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
        Task<bool> UpdateRecipeMeta(RamseyContext context,RecipeMetaDto recipe);
        Task<Dictionary<string,bool>> UpdateRecipeDatabaseAsync(RamseyContext context,List<RecipeMetaDto> recipes);
    }
}
