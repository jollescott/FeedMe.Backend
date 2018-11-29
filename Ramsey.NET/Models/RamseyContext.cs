using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Models
{
    public class RamseyContext : DbContext
    {
        public RamseyContext(DbContextOptions<RamseyContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Recipe>()
                .HasIndex(p => p.NativeID)
                .IsUnique();
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipePart> RecipeParts { get; set; }
    }
}
