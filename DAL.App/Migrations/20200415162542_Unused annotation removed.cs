using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.App.Migrations
{
    public partial class Unusedannotationremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectInSchedules_Subjects_SubjectId",
                table: "SubjectInSchedules");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectInSchedules_Subjects_SubjectId",
                table: "SubjectInSchedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectInSchedules_Subjects_SubjectId",
                table: "SubjectInSchedules");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectInSchedules_Subjects_SubjectId",
                table: "SubjectInSchedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
