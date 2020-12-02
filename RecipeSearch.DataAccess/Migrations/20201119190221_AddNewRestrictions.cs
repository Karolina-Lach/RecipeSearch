using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeSearch.DataAccess.Migrations
{
    public partial class AddNewRestrictions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PluralName",
                table: "Units",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PluralName",
                table: "Products",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Measurement",
                table: "Ingredients",
                type: "decimal(5, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PluralName",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PluralName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Measurement",
                table: "Ingredients",
                type: "decimal(5, 1)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)");
        }
    }
}
