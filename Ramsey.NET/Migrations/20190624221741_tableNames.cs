using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Ramsey.NET.Migrations
{
    public partial class tableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeParts_Ingredients_IngredientId",
                table: "RecipeParts");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeParts_Recipes_RecipeId",
                table: "RecipeParts");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Recipes_RecipeId",
                table: "RecipeTags");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Tags_TagId",
                table: "RecipeTags");

            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeTags",
                table: "RecipeTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeParts",
                table: "RecipeParts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientSynonyms",
                table: "IngredientSynonyms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FailedRecipes",
                table: "FailedRecipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BadWords",
                table: "BadWords");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "tags");

            migrationBuilder.RenameTable(
                name: "RecipeTags",
                newName: "recipeTags");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "recipes");

            migrationBuilder.RenameTable(
                name: "RecipeParts",
                newName: "recipeParts");

            migrationBuilder.RenameTable(
                name: "IngredientSynonyms",
                newName: "ingredientSynonyms");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "ingredients");

            migrationBuilder.RenameTable(
                name: "FailedRecipes",
                newName: "failedRecipes");

            migrationBuilder.RenameTable(
                name: "BadWords",
                newName: "badWords");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeTags_TagId",
                table: "recipeTags",
                newName: "IX_recipeTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeTags_RecipeId",
                table: "recipeTags",
                newName: "IX_recipeTags_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeParts_RecipeId",
                table: "recipeParts",
                newName: "IX_recipeParts_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeParts_IngredientId",
                table: "recipeParts",
                newName: "IX_recipeParts_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tags",
                table: "tags",
                column: "TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_recipeTags",
                table: "recipeTags",
                column: "RecipeTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_recipes",
                table: "recipes",
                column: "RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_recipeParts",
                table: "recipeParts",
                column: "RecipePartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ingredientSynonyms",
                table: "ingredientSynonyms",
                column: "IngredientSynonymId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ingredients",
                table: "ingredients",
                column: "IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_failedRecipes",
                table: "failedRecipes",
                column: "FailedRecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_badWords",
                table: "badWords",
                column: "BadWordId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipeParts_ingredients_IngredientId",
                table: "recipeParts",
                column: "IngredientId",
                principalTable: "ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_recipeParts_recipes_RecipeId",
                table: "recipeParts",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_recipeTags_recipes_RecipeId",
                table: "recipeTags",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_recipeTags_tags_TagId",
                table: "recipeTags",
                column: "TagId",
                principalTable: "tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipeParts_ingredients_IngredientId",
                table: "recipeParts");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeParts_recipes_RecipeId",
                table: "recipeParts");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeTags_recipes_RecipeId",
                table: "recipeTags");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeTags_tags_TagId",
                table: "recipeTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tags",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_recipeTags",
                table: "recipeTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_recipes",
                table: "recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_recipeParts",
                table: "recipeParts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ingredientSynonyms",
                table: "ingredientSynonyms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ingredients",
                table: "ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_failedRecipes",
                table: "failedRecipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_badWords",
                table: "badWords");

            migrationBuilder.RenameTable(
                name: "tags",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "recipeTags",
                newName: "RecipeTags");

            migrationBuilder.RenameTable(
                name: "recipes",
                newName: "Recipes");

            migrationBuilder.RenameTable(
                name: "recipeParts",
                newName: "RecipeParts");

            migrationBuilder.RenameTable(
                name: "ingredientSynonyms",
                newName: "IngredientSynonyms");

            migrationBuilder.RenameTable(
                name: "ingredients",
                newName: "Ingredients");

            migrationBuilder.RenameTable(
                name: "failedRecipes",
                newName: "FailedRecipes");

            migrationBuilder.RenameTable(
                name: "badWords",
                newName: "BadWords");

            migrationBuilder.RenameIndex(
                name: "IX_recipeTags_TagId",
                table: "RecipeTags",
                newName: "IX_RecipeTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_recipeTags_RecipeId",
                table: "RecipeTags",
                newName: "IX_RecipeTags_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_recipeParts_RecipeId",
                table: "RecipeParts",
                newName: "IX_RecipeParts_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_recipeParts_IngredientId",
                table: "RecipeParts",
                newName: "IX_RecipeParts_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeTags",
                table: "RecipeTags",
                column: "RecipeTagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes",
                column: "RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeParts",
                table: "RecipeParts",
                column: "RecipePartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientSynonyms",
                table: "IngredientSynonyms",
                column: "IngredientSynonymId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FailedRecipes",
                table: "FailedRecipes",
                column: "FailedRecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BadWords",
                table: "BadWords",
                column: "BadWordId");

            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeParts_Ingredients_IngredientId",
                table: "RecipeParts",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeParts_Recipes_RecipeId",
                table: "RecipeParts",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Recipes_RecipeId",
                table: "RecipeTags",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Tags_TagId",
                table: "RecipeTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
