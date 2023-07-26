using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ramsey.Core.Models;
using Ramsey.NET.Models;

namespace Ramsey.Core.Interfaces
{
    public interface IRamseyContext : IDisposable
    {
        DbSet<Ingredient> Ingredients { get; }
        DbSet<RecipeMeta> Recipes { get; }
        DbSet<RecipePart> RecipeParts { get; }
        DbSet<IngredientSynonym> IngredientSynonyms { get; set; }
        DbSet<BadWord> BadWords { get; set; }
        DbSet<FailedRecipe> FailedRecipes { get; set; }

        DbSet<RecipeTag> RecipeTags { get; set; }
        DbSet<Tag> Tags { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}