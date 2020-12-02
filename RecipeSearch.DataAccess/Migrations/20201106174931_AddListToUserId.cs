using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeSearch.DataAccess.Migrations
{
    public partial class AddListToUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ListOfFavourites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListOfFavourites_ApplicationUserId",
                table: "ListOfFavourites",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListOfFavourites_AspNetUsers_ApplicationUserId",
                table: "ListOfFavourites",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListOfFavourites_AspNetUsers_ApplicationUserId",
                table: "ListOfFavourites");

            migrationBuilder.DropIndex(
                name: "IX_ListOfFavourites_ApplicationUserId",
                table: "ListOfFavourites");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ListOfFavourites");
        }
    }
}
