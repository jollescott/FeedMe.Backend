﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Ramsey.NET.Implementations;

namespace Ramsey.NET.Migrations
{
    [DbContext(typeof(RamseyContext))]
    [Migration("20190624215800_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Ramsey.Core.Models.RecipeTag", b =>
                {
                    b.Property<int>("RecipeTagId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RecipeId");

                    b.Property<int>("TagId");

                    b.HasKey("RecipeTagId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("TagId");

                    b.ToTable("RecipeTags");
                });

            modelBuilder.Entity("Ramsey.Core.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Locale")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Name");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Ramsey.NET.Models.AdminUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("AdminUsers");
                });

            modelBuilder.Entity("Ramsey.NET.Models.BadWord", b =>
                {
                    b.Property<int>("BadWordId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Locale")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Word");

                    b.HasKey("BadWordId");

                    b.ToTable("BadWords");
                });

            modelBuilder.Entity("Ramsey.NET.Models.FailedRecipe", b =>
                {
                    b.Property<int>("FailedRecipeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message");

                    b.Property<string>("Url");

                    b.HasKey("FailedRecipeId");

                    b.ToTable("FailedRecipes");
                });

            modelBuilder.Entity("Ramsey.NET.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("IngredientName");

                    b.Property<int>("Locale")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.HasKey("IngredientId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Ramsey.NET.Models.IngredientSynonym", b =>
                {
                    b.Property<int>("IngredientSynonymId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Correct");

                    b.Property<int>("Locale")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Wrong");

                    b.HasKey("IngredientSynonymId");

                    b.ToTable("IngredientSynonyms");
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipeMeta", b =>
                {
                    b.Property<string>("RecipeId");

                    b.Property<string>("Image");

                    b.Property<int>("Locale")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Name");

                    b.Property<int>("Owner");

                    b.Property<string>("OwnerLogo");

                    b.Property<double>("Rating");

                    b.Property<string>("Source");

                    b.HasKey("RecipeId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipePart", b =>
                {
                    b.Property<int>("RecipePartId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IngredientId");

                    b.Property<float>("Quantity");

                    b.Property<string>("RecipeId");

                    b.Property<string>("Unit");

                    b.HasKey("RecipePartId");

                    b.HasIndex("IngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeParts");
                });

            modelBuilder.Entity("Ramsey.Core.Models.RecipeTag", b =>
                {
                    b.HasOne("Ramsey.NET.Models.RecipeMeta", "Recipe")
                        .WithMany("RecipeTags")
                        .HasForeignKey("RecipeId");

                    b.HasOne("Ramsey.Core.Models.Tag", "Tag")
                        .WithMany("RecipeTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipePart", b =>
                {
                    b.HasOne("Ramsey.NET.Models.Ingredient", "Ingredient")
                        .WithMany("RecipeParts")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ramsey.NET.Models.RecipeMeta", "Recipe")
                        .WithMany("RecipeParts")
                        .HasForeignKey("RecipeId");
                });
#pragma warning restore 612, 618
        }
    }
}
