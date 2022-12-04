using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidenceProject.Migrations
{
    /// <inheritdoc />
    public partial class ProjectApplicants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUser_projects_Projectsid",
                table: "ProjectUser");

            migrationBuilder.RenameColumn(
                name: "Projectsid",
                table: "ProjectUser",
                newName: "Assigness");

            migrationBuilder.AddColumn<int>(
                name: "Applicants",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Assigness",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectUser1",
                columns: table => new
                {
                    Applicants = table.Column<int>(type: "int", nullable: false),
                    applicantsid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser1", x => new { x.Applicants, x.applicantsid });
                    table.ForeignKey(
                        name: "FK_ProjectUser1_User_applicantsid",
                        column: x => x.applicantsid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser1_projects_Applicants",
                        column: x => x.Applicants,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser1_applicantsid",
                table: "ProjectUser1",
                column: "applicantsid");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUser_projects_Assigness",
                table: "ProjectUser",
                column: "Assigness",
                principalTable: "projects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUser_projects_Assigness",
                table: "ProjectUser");

            migrationBuilder.DropTable(
                name: "ProjectUser1");

            migrationBuilder.DropColumn(
                name: "Applicants",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Assigness",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Assigness",
                table: "ProjectUser",
                newName: "Projectsid");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUser_projects_Projectsid",
                table: "ProjectUser",
                column: "Projectsid",
                principalTable: "projects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
