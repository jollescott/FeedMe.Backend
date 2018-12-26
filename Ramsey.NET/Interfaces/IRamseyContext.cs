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
        DbSet<RecipePart> RecipeParts { get;  }
        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}