﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ramsey.NET.Implementations;
using Ramsey.Shared.Enums;

namespace Ramsey.NET.Migrations
{
    [DbContext(typeof(RamseyContext))]
    [Migration("20190117204847_AdminUser")]
    partial class AdminUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ramsey.NET.Models.AdminUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("AdminUsers");
                });

            modelBuilder.Entity("Ramsey.NET.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IngredientName");

                    b.HasKey("IngredientId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Ramsey.NET.Models.RamseyUser", b =>
                {
                    b.Property<string>("UserId");

                    b.HasKey("UserId");

                    b.ToTable("RamseyUsers");
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipeFavorite", b =>
                {
                    b.Property<int>("RecipeFavoriteId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RecipeId");

                    b.Property<string>("UserId");

                    b.HasKey("RecipeFavoriteId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("UserId");

                    b.ToTable("RecipeFavorites");
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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IngredientId");

                    b.Property<float>("Quantity");

                    b.Property<string>("RecipeId");

                    b.Property<string>("Unit");

                    b.HasKey("RecipePartId");

                    b.HasIndex("IngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeParts");
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipeRating", b =>
                {
                    b.Property<int>("RecipeRatingId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RecipeId");

                    b.Property<double>("Score");

                    b.Property<string>("UserId");

                    b.HasKey("RecipeRatingId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("UserId");

                    b.ToTable("RecipeRatings");
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipeFavorite", b =>
                {
                    b.HasOne("Ramsey.NET.Models.RecipeMeta", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId");

                    b.HasOne("Ramsey.NET.Models.RamseyUser", "Ingredient")
                        .WithMany("RecipeFavorites")
                        .HasForeignKey("UserId");
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

            modelBuilder.Entity("Ramsey.NET.Models.RecipeRating", b =>
                {
                    b.HasOne("Ramsey.NET.Models.RecipeMeta", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId");

                    b.HasOne("Ramsey.NET.Models.RamseyUser", "Ingredient")
                        .WithMany("RecipeRatings")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
