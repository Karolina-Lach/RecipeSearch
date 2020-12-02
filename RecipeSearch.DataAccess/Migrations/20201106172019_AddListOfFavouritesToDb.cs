using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeSearch.DataAccess.Migrations
{
    public partial class AddListOfFavouritesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListOfFavourites",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfFavourites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListOfFavouritesRecipe",
                columns: table => new
                {
                    RecipeId = table.Column<int>(nullable: false),
                    ListOfFavouritesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfFavouritesRecipe", x => new { x.ListOfFavouritesId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_ListOfFavouritesRecipe_ListOfFavourites_ListOfFavouritesId",
                        column: x => x.ListOfFavouritesId,
                        principalTable: "ListOfFavourites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListOfFavouritesRecipe_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListOfFavouritesRecipe_RecipeId",
                table: "ListOfFavouritesRecipe",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListOfFavouritesRecipe");

            migrationBuilder.DropTable(
                name: "ListOfFavourites");
        }
    }
}
