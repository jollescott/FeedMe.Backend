﻿// <auto-generated />
using System;
using GusteauSharp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ramsey.NET.Migrations
{
    [DbContext(typeof(RamseyContext))]
    [Migration("20181024120620_DirectionMigration")]
    partial class DirectionMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GusteauSharp.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("IngredientID");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("GusteauSharp.Models.Recipe", b =>
                {
                    b.Property<int>("RecipeID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<string>("Desc");

                    b.Property<double>("Fat");

                    b.Property<string>("Name");

                    b.Property<double>("Protein");

                    b.Property<double>("Rating");

                    b.Property<double>("Sodium");

                    b.HasKey("RecipeID");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("GusteauSharp.Models.RecipePart", b =>
                {
                    b.Property<int>("RecipePartID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IngredientID");

                    b.Property<double>("Quantity");

                    b.Property<int>("RecipeID");

                    b.Property<string>("Unit");

                    b.HasKey("RecipePartID");

                    b.HasIndex("IngredientID");

                    b.HasIndex("RecipeID");

                    b.ToTable("RecipeParts");
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipeCategory", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("RecipeID");

                    b.HasKey("CategoryID");

                    b.HasIndex("RecipeID");

                    b.ToTable("RecipeCategories");
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipeDirection", b =>
                {
                    b.Property<int>("DirectionID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Instruction");

                    b.Property<int>("RecipeID");

                    b.HasKey("DirectionID");

                    b.HasIndex("RecipeID");

                    b.ToTable("RecipeDirections");
                });

            modelBuilder.Entity("GusteauSharp.Models.RecipePart", b =>
                {
                    b.HasOne("GusteauSharp.Models.Ingredient", "Ingredient")
                        .WithMany("RecipeParts")
                        .HasForeignKey("IngredientID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GusteauSharp.Models.Recipe", "Recipe")
                        .WithMany("RecipeParts")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipeCategory", b =>
                {
                    b.HasOne("GusteauSharp.Models.Recipe", "Recipe")
                        .WithMany("Categories")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ramsey.NET.Models.RecipeDirection", b =>
                {
                    b.HasOne("GusteauSharp.Models.Recipe", "Recipe")
                        .WithMany("Directions")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
