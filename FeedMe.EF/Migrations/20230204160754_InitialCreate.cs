using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramsey.NET.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "badWords",
                columns: table => new
                {
                    BadWordId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Word = table.Column<string>(type: "TEXT", nullable: true),
                    Locale = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badWords", x => x.BadWordId);
                });

            migrationBuilder.CreateTable(
                name: "failedRecipes",
                columns: table => new
                {
                    FailedRecipeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_failedRecipes", x => x.FailedRecipeId);
                });

            migrationBuilder.CreateTable(
                name: "ingredients",
                columns: table => new
                {
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IngredientName = table.Column<string>(type: "TEXT", nullable: true),
                    Locale = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredients", x => x.IngredientId);
                });

            migrationBuilder.CreateTable(
                name: "ingredientSynonyms",
                columns: table => new
                {
                    IngredientSynonymId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Correct = table.Column<string>(type: "TEXT", nullable: true),
                    Wrong = table.Column<string>(type: "TEXT", nullable: true),
                    Locale = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredientSynonyms", x => x.IngredientSynonymId);
                });

            migrationBuilder.CreateTable(
                name: "recipes",
                columns: table => new
                {
                    RecipeId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: true),
                    Owner = table.Column<int>(type: "INTEGER", nullable: false),
                    Locale = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    OwnerLogo = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    Rating = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipes", x => x.RecipeId);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Locale = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "recipeParts",
                columns: table => new
                {
                    RecipePartId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<string>(type: "TEXT", nullable: true),
                    Unit = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipeParts", x => x.RecipePartId);
                    table.ForeignKey(
                        name: "FK_recipeParts_ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recipeParts_recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "recipes",
                        principalColumn: "RecipeId");
                });

            migrationBuilder.CreateTable(
                name: "recipeTags",
                columns: table => new
                {
                    RecipeTagId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<string>(type: "TEXT", nullable: true),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipeTags", x => x.RecipeTagId);
                    table.ForeignKey(
                        name: "FK_recipeTags_recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "recipes",
                        principalColumn: "RecipeId");
                    table.ForeignKey(
                        name: "FK_recipeTags_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recipeParts_IngredientId",
                table: "recipeParts",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_recipeParts_RecipeId",
                table: "recipeParts",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_recipeTags_RecipeId",
                table: "recipeTags",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_recipeTags_TagId",
                table: "recipeTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "badWords");

            migrationBuilder.DropTable(
                name: "failedRecipes");

            migrationBuilder.DropTable(
                name: "ingredientSynonyms");

            migrationBuilder.DropTable(
                name: "recipeParts");

            migrationBuilder.DropTable(
                name: "recipeTags");

            migrationBuilder.DropTable(
                name: "ingredients");

            migrationBuilder.DropTable(
                name: "recipes");

            migrationBuilder.DropTable(
                name: "tags");
        }
    }
}
