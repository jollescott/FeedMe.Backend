using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GusteauSharp.Models
{
    public class RamseyContext : DbContext
    {
        public RamseyContext(DbContextOptions<RamseyContext> options): base(options)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipePart> RecipeParts { get; set; }
        public DbSet<RecipeDirection> RecipeDirections { get; set; }
        public DbSet<RecipeCategory> RecipeCategories { get; set; }
    }
}
