using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ramsey.NET.Migrations
{
    public partial class AdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeFavorites_Users_UserId",
                table: "RecipeFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeRatings_Users_UserId",
                table: "RecipeRatings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RamseyUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RamseyUsers", x => x.UserId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeFavorites_RamseyUsers_UserId",
                table: "RecipeFavorites",
                column: "UserId",
                principalTable: "RamseyUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeRatings_RamseyUsers_UserId",
                table: "RecipeRatings",
                column: "UserId",
                principalTable: "RamseyUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeFavorites_RamseyUsers_UserId",
                table: "RecipeFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeRatings_RamseyUsers_UserId",
                table: "RecipeRatings");

            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "RamseyUsers");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeFavorites_Users_UserId",
                table: "RecipeFavorites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeRatings_Users_UserId",
                table: "RecipeRatings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
