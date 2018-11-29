﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ramsey.NET.Models;

namespace Ramsey.NET.Migrations
{
    [DbContext(typeof(RamseyContext))]
    partial class RamseyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ramsey.NET.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("RecipeID");

                    b.HasKey("IngredientID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.HasIndex("RecipeID");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Ramsey.NET.Models.Recipe", b =>
                {
                    b.Property<int>("RecipeID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<int>("NativeID");

                    b.Property<string>("Owner");

                    b.Property<string>("Source");

                    b.HasKey("RecipeID");

                    b.HasIndex("NativeID")
                        .IsUnique();

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipePart", b =>
                {
                    b.Property<int>("RecipePartID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IngredientID");

                    b.Property<int>("RecipeID");

                    b.HasKey("RecipePartID");

                    b.HasIndex("IngredientID");

                    b.HasIndex("RecipeID");

                    b.ToTable("RecipeParts");
                });

            modelBuilder.Entity("Ramsey.NET.Models.Ingredient", b =>
                {
                    b.HasOne("Ramsey.NET.Models.Recipe")
                        .WithMany("RecipeParts")
                        .HasForeignKey("RecipeID");
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipePart", b =>
                {
                    b.HasOne("Ramsey.NET.Models.Ingredient", "Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ramsey.NET.Models.Recipe", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
