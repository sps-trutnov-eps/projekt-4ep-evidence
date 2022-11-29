using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidenceProject.Migrations
{
    /// <inheritdoc />
    public partial class techdesc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_dialCodes_Technology",
                table: "projects");

            migrationBuilder.DropIndex(
                name: "IX_projects_Technology",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "Technology",
                table: "projects");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "projectDescription",
                table: "projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Technology",
                table: "dialCodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DialCodeProject",
                columns: table => new
                {
                    Technology = table.Column<int>(type: "int", nullable: false),
                    projectTechnologyid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialCodeProject", x => new { x.Technology, x.projectTechnologyid });
                    table.ForeignKey(
                        name: "FK_DialCodeProject_dialCodes_projectTechnologyid",
                        column: x => x.projectTechnologyid,
                        principalTable: "dialCodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DialCodeProject_projects_Technology",
                        column: x => x.Technology,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DialCodeProject_projectTechnologyid",
                table: "DialCodeProject",
                column: "projectTechnologyid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DialCodeProject");

            migrationBuilder.DropColumn(
                name: "projectDescription",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "Technology",
                table: "dialCodes");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Technology",
                table: "projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_projects_Technology",
                table: "projects",
                column: "Technology");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_dialCodes_Technology",
                table: "projects",
                column: "Technology",
                principalTable: "dialCodes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
