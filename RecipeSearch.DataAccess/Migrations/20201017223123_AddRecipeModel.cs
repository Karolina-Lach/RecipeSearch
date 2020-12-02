using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeSearch.DataAccess.Migrations
{
    public partial class AddRecipeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "Steps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "Ingredients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(maxLength: 200, nullable: true),
                    PrepTime = table.Column<int>(nullable: false),
                    Servings = table.Column<int>(nullable: false),
                    Source = table.Column<string>(maxLength: 50, nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Diary = table.Column<bool>(nullable: false),
                    Gluten = table.Column<bool>(nullable: false),
                    Vegan = table.Column<bool>(nullable: false),
                    Vegeterian = table.Column<bool>(nullable: false),
                    Healthy = table.Column<bool>(nullable: false),
                    CuisinesString = table.Column<string>(nullable: true),
                    MealTypesString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Steps_RecipeId",
                table: "Steps",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Recipes_RecipeId",
                table: "Ingredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Recipes_RecipeId",
                table: "Steps",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Recipes_RecipeId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Recipes_RecipeId",
                table: "Steps");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Steps_RecipeId",
                table: "Steps");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Steps");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Ingredients");
        }
    }
}
