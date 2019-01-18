using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ramsey.NET.Models;

namespace Ramsey.NET.Interfaces
{
    public interface IRamseyContext : IDisposable
    {
        DbSet<Ingredient> Ingredients { get; }
        DbSet<RecipeMeta> Recipes { get; }
        DbSet<RecipePart> RecipeParts { get; }
        DbSet<RamseyUser> RamseyUsers { get; }
        DbSet<AdminUser> AdminUsers { get; }
        DbSet<RecipeFavorite> RecipeFavorites { get; }
        DbSet<RecipeRating> RecipeRatings { get; }
        DbSet<IngredientSynonym> IngredientSynonyms { get; set; }
        DbSet<BadWord> BadWords { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}