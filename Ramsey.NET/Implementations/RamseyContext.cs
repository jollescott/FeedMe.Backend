using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsey.NET.Interfaces;

namespace Ramsey.NET.Implementations
{
    public class RamseyContext : DbContext, IRamseyContext
    {
        public RamseyContext(DbContextOptions<RamseyContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>()
                .HasIndex(x => x.IngredientID);

            modelBuilder.Entity<RecipeMeta>()
                .HasIndex(x => x.Owner);

            modelBuilder.Entity<RecipePart>()
                .HasIndex(x => new { x.IngredientId, x.RecipeId });
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeMeta> Recipes { get; set; }
        public DbSet<RecipePart> RecipeParts { get; set; }
    }
}
