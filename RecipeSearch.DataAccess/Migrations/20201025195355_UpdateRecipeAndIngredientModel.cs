using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeSearch.DataAccess.Migrations
{
    public partial class UpdateRecipeAndIngredientModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diary",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Gluten",
                table: "Recipes");

            migrationBuilder.AddColumn<bool>(
                name: "NoDairy",
                table: "Recipes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NoGluten",
                table: "Recipes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Ingredients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoDairy",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "NoGluten",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Ingredients");

            migrationBuilder.AddColumn<bool>(
                name: "Diary",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Gluten",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
