using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Models;
using Ramsey.NET.Interfaces;
using Ramsey.Shared.Enums;

namespace Ramsey.NET.Implementations
{
    public class RamseyContext : DbContext, IRamseyContext
    {
        public RamseyContext(DbContextOptions<RamseyContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Ingredient
            modelBuilder.Entity<Ingredient>()
                .HasKey(x => x.IngredientId);

            modelBuilder.Entity<Ingredient>()
                .Property(x => x.IngredientName)
                .ValueGeneratedNever();

            //User 
            modelBuilder.Entity<RamseyUser>()
                .HasKey(x => x.UserId);

            modelBuilder.Entity<RamseyUser>()
                .Property(x => x.UserId)
                .ValueGeneratedNever();

            //Recipe Meta
            modelBuilder.Entity<RecipeMeta>()
                .HasKey(x => x.RecipeId);

            modelBuilder.Entity<RecipeMeta>()
                .Property(x => x.RecipeId)
                .ValueGeneratedNever();

            modelBuilder.Entity<RecipeMeta>()
                .Property(x => x.Locale)
                .HasDefaultValue(RamseyLocale.Swedish);

            modelBuilder.Entity<BadWord>()
                .Property(x => x.Locale)
                .HasDefaultValue(RamseyLocale.Swedish);

            modelBuilder.Entity<IngredientSynonym>()
                .Property(x => x.Locale)
                .HasDefaultValue(RamseyLocale.Swedish);
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeMeta> Recipes { get; set; }
        public DbSet<RecipePart> RecipeParts { get; set; }
        public DbSet<RecipeFavorite> RecipeFavorites { get; set; }
        public DbSet<RecipeRating> RecipeRatings { get; set; }
        public DbSet<RamseyUser> RamseyUsers { get; set; }

        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<IngredientSynonym> IngredientSynonyms { get; set; }
        public DbSet<BadWord> BadWords { get; set; }
    }
}
