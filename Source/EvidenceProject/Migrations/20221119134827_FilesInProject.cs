using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidenceProject.Migrations
{
    public partial class FilesInProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Projectid",
                table: "files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_files_Projectid",
                table: "files",
                column: "Projectid");

            migrationBuilder.AddForeignKey(
                name: "FK_files_projects_Projectid",
                table: "files",
                column: "Projectid",
                principalTable: "projects",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_projects_Projectid",
                table: "files");

            migrationBuilder.DropIndex(
                name: "IX_files_Projectid",
                table: "files");

            migrationBuilder.DropColumn(
                name: "Projectid",
                table: "files");
        }
    }
}
