using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeSearch.DataAccess.Migrations
{
    public partial class updateIngredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Ingredients",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int?");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Ingredients",
                type: "int?",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
