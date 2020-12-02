using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeSearch.DataAccess.Migrations
{
    public partial class UpdateUnitModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Units",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PluralName",
                table: "Units",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PluralName",
                table: "Units");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
