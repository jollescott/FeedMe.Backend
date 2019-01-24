using Microsoft.EntityFrameworkCore.Migrations;

namespace Ramsey.NET.Migrations
{
    public partial class FailedRecipes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FailedRecipes",
                columns: table => new
                {
                    FailedRecipeId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedRecipes", x => x.FailedRecipeId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FailedRecipes");
        }
    }
}
