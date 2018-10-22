using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GusteauSharp.Models
{
    public class GusteauContext : DbContext
    {
        public GusteauContext(DbContextOptions<GusteauContext> options): base(options)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipePart> RecipeParts { get; set; }
    }
}
