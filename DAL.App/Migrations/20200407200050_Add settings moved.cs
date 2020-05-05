using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.App.Migrations
{
    public partial class Addsettingsmoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowAddSeconds",
                table: "Screens");

            migrationBuilder.AddColumn<int>(
                name: "ShowAddSeconds",
                table: "PictureInScreens",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowAddSeconds",
                table: "PictureInScreens");

            migrationBuilder.AddColumn<int>(
                name: "ShowAddSeconds",
                table: "Screens",
                type: "int",
                nullable: true);
        }
    }
}
