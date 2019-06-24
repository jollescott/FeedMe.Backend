using Microsoft.EntityFrameworkCore;
using Ramsey.NET.Models;
using Ramsey.NET.Interfaces;
using Ramsey.Shared.Enums;
using Ramsey.Core.Models;

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

            //Error Reporting
            modelBuilder.Entity<FailedRecipe>()
                .HasKey(x => x.FailedRecipeId);

            modelBuilder.Entity<FailedRecipe>()
               .ToTable("failedRecipes");

            //Ingredient
            modelBuilder.Entity<Ingredient>()
                .HasKey(x => x.IngredientId);

            modelBuilder.Entity<Ingredient>()
                .Property(x => x.IngredientName)
                .ValueGeneratedNever();

            modelBuilder.Entity<Ingredient>()
              .ToTable("ingredients");

            //Tags
            modelBuilder.Entity<Tag>()
                .HasKey(x => x.TagId);

            modelBuilder.Entity<RecipeTag>()
                .HasKey(x => x.RecipeTagId);

            modelBuilder.Entity<RecipeTag>()
                .HasOne(x => x.Recipe)
                .WithMany(x => x.RecipeTags)
                .HasForeignKey(x => x.RecipeId);

            modelBuilder.Entity<RecipeTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.RecipeTags)
                .HasForeignKey(x => x.TagId);

            modelBuilder.Entity<Tag>()
              .ToTable("tags");

            modelBuilder.Entity<RecipeTag>()
              .ToTable("recipeTags");

            //Recipe Meta
            modelBuilder.Entity<RecipeMeta>()
                .HasKey(x => x.RecipeId);

            modelBuilder.Entity<RecipeMeta>()
                .Property(x => x.RecipeId)
                .ValueGeneratedNever();

            modelBuilder.Entity<RecipeMeta>()
              .ToTable("recipes");

            //Locale setup
            modelBuilder.Entity<RecipeMeta>()
                .Property(x => x.Locale)
                .HasDefaultValue(RamseyLocale.Swedish);

            modelBuilder.Entity<BadWord>()
                .Property(x => x.Locale)
                .HasDefaultValue(RamseyLocale.Swedish);

            modelBuilder.Entity<IngredientSynonym>()
                .Property(x => x.Locale)
                .HasDefaultValue(RamseyLocale.Swedish);

            modelBuilder.Entity<Tag>()
                .Property(x => x.Locale)
                .HasDefaultValue(RamseyLocale.Swedish);

            modelBuilder.Entity<Ingredient>()
                .Property(x => x.Locale)
                .HasDefaultValue(RamseyLocale.Swedish);

            modelBuilder.Entity<RecipePart>()
              .ToTable("recipeParts");

            modelBuilder.Entity<IngredientSynonym>()
              .ToTable("ingredientSynonyms");

            modelBuilder.Entity<BadWord>()
              .ToTable("badWords");
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeMeta> Recipes { get; set; }
        public DbSet<RecipePart> RecipeParts { get; set; }
        public DbSet<FailedRecipe> FailedRecipes { get; set; }
        public DbSet<IngredientSynonym> IngredientSynonyms { get; set; }
        public DbSet<BadWord> BadWords { get; set; }

        public DbSet<RecipeTag> RecipeTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
