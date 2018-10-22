using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ramsey.NET.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientID);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    NumberOfServings = table.Column<int>(nullable: false),
                    TotalTimeInSeconds = table.Column<double>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    Cuisine = table.Column<string>(nullable: true),
                    Course = table.Column<string>(nullable: true),
                    MediumImage = table.Column<string>(nullable: true),
                    SmallImage = table.Column<string>(nullable: true),
                    Bitter = table.Column<float>(nullable: false),
                    Meaty = table.Column<float>(nullable: false),
                    Piquant = table.Column<float>(nullable: false),
                    Salty = table.Column<float>(nullable: false),
                    Sour = table.Column<float>(nullable: false),
                    Sweet = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeID);
                });

            migrationBuilder.CreateTable(
                name: "RecipeParts",
                columns: table => new
                {
                    RecipePartID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IngredientID = table.Column<int>(nullable: false),
                    RecipeID = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeParts", x => x.RecipePartID);
                    table.ForeignKey(
                        name: "FK_RecipeParts_Ingredients_IngredientID",
                        column: x => x.IngredientID,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeParts_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "RecipeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeParts_IngredientID",
                table: "RecipeParts",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeParts_RecipeID",
                table: "RecipeParts",
                column: "RecipeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeParts");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
