﻿// <auto-generated />
using GusteauSharp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ramsey.NET.Migrations
{
    [DbContext(typeof(GusteauContext))]
    [Migration("20181022144053_InitialMigration")]
    partial class InitialMigration
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

                    b.Property<float>("Bitter");

                    b.Property<string>("Course");

                    b.Property<string>("Cuisine");

                    b.Property<float>("Meaty");

                    b.Property<string>("MediumImage");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfServings");

                    b.Property<float>("Piquant");

                    b.Property<int>("Rating");

                    b.Property<float>("Salty");

                    b.Property<string>("SmallImage");

                    b.Property<float>("Sour");

                    b.Property<float>("Sweet");

                    b.Property<double>("TotalTimeInSeconds");

                    b.HasKey("RecipeID");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("GusteauSharp.Models.RecipePart", b =>
                {
                    b.Property<int>("RecipePartID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IngredientID");

                    b.Property<decimal>("Quantity");

                    b.Property<int>("RecipeID");

                    b.HasKey("RecipePartID");

                    b.HasIndex("IngredientID");

                    b.HasIndex("RecipeID");

                    b.ToTable("RecipeParts");
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
#pragma warning restore 612, 618
        }
    }
}
