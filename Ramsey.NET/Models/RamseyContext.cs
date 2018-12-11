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

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeMeta> Recipes { get; set; }
        public DbSet<RecipePart> RecipeParts { get; set; }
    }
}
