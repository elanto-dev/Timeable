using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.App.Migrations
{
    public partial class UniqueIdtoscreen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentifier",
                table: "Screens",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "Schedules",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Screens_UniqueIdentifier",
                table: "Screens",
                column: "UniqueIdentifier",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Screens_UniqueIdentifier",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "UniqueIdentifier",
                table: "Screens");

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }
    }
}
