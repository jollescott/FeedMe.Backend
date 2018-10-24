using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ramsey.NET.Migrations
{
    public partial class DirectionMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bitter",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Course",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Cuisine",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Meaty",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "MediumImage",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "NumberOfServings",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Piquant",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Salty",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Sour",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Sweet",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "TotalTimeInSeconds",
                table: "Recipes",
                newName: "Sodium");

            migrationBuilder.RenameColumn(
                name: "SmallImage",
                table: "Recipes",
                newName: "Desc");

            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "Recipes",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Recipes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Fat",
                table: "Recipes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Protein",
                table: "Recipes",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "RecipeParts",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "RecipeParts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecipeCategories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    RecipeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeCategories", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_RecipeCategories_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "RecipeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeDirections",
                columns: table => new
                {
                    DirectionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Instruction = table.Column<string>(nullable: true),
                    RecipeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeDirections", x => x.DirectionID);
                    table.ForeignKey(
                        name: "FK_RecipeDirections_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "RecipeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeCategories_RecipeID",
                table: "RecipeCategories",
                column: "RecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeDirections_RecipeID",
                table: "RecipeDirections",
                column: "RecipeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeCategories");

            migrationBuilder.DropTable(
                name: "RecipeDirections");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Fat",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "RecipeParts");

            migrationBuilder.RenameColumn(
                name: "Sodium",
                table: "Recipes",
                newName: "TotalTimeInSeconds");

            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "Recipes",
                newName: "SmallImage");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Recipes",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<float>(
                name: "Bitter",
                table: "Recipes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cuisine",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Meaty",
                table: "Recipes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "MediumImage",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfServings",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Piquant",
                table: "Recipes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Salty",
                table: "Recipes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Sour",
                table: "Recipes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Sweet",
                table: "Recipes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "RecipeParts",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
