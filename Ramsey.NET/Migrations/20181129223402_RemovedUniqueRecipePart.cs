using Microsoft.EntityFrameworkCore.Migrations;

namespace Ramsey.NET.Migrations
{
    public partial class RemovedUniqueRecipePart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipeParts_RecipeID_IngredientID",
                table: "RecipeParts");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeParts_RecipeID",
                table: "RecipeParts",
                column: "RecipeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipeParts_RecipeID",
                table: "RecipeParts");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeParts_RecipeID_IngredientID",
                table: "RecipeParts",
                columns: new[] { "RecipeID", "IngredientID" },
                unique: true);
        }
    }
}
