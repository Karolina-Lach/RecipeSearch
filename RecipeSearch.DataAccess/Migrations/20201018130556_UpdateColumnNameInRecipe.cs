using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeSearch.DataAccess.Migrations
{
    public partial class UpdateColumnNameInRecipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vegeterian",
                table: "Recipes");

            migrationBuilder.AddColumn<bool>(
                name: "Vegetarian",
                table: "Recipes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vegetarian",
                table: "Recipes");

            migrationBuilder.AddColumn<bool>(
                name: "Vegeterian",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
