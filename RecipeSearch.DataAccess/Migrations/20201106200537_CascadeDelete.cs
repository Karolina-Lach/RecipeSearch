using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeSearch.DataAccess.Migrations
{
    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListOfFavourites_AspNetUsers_ApplicationUserId",
                table: "ListOfFavourites");

            migrationBuilder.AddForeignKey(
                name: "FK_ListOfFavourites_AspNetUsers_ApplicationUserId",
                table: "ListOfFavourites",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListOfFavourites_AspNetUsers_ApplicationUserId",
                table: "ListOfFavourites");

            migrationBuilder.AddForeignKey(
                name: "FK_ListOfFavourites_AspNetUsers_ApplicationUserId",
                table: "ListOfFavourites",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
