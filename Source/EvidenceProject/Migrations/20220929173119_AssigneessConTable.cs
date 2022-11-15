using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidenceProject.Migrations
{
    public partial class AssigneessConTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dialCodes_projects_Achivements",
                table: "dialCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_User_projects_Projectid",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Projectid",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_dialCodes_Achivements",
                table: "dialCodes");

            migrationBuilder.DropColumn(
                name: "Projectid",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Achivements",
                table: "dialCodes");

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    Projectsid = table.Column<int>(type: "int", nullable: false),
                    assigneesid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.Projectsid, x.assigneesid });
                    table.ForeignKey(
                        name: "FK_ProjectUser_projects_Projectsid",
                        column: x => x.Projectsid,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_User_assigneesid",
                        column: x => x.assigneesid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_assigneesid",
                table: "ProjectUser",
                column: "assigneesid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUser");

            migrationBuilder.AddColumn<int>(
                name: "Projectid",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Achivements",
                table: "dialCodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Projectid",
                table: "User",
                column: "Projectid");

            migrationBuilder.CreateIndex(
                name: "IX_dialCodes_Achivements",
                table: "dialCodes",
                column: "Achivements");

            migrationBuilder.AddForeignKey(
                name: "FK_dialCodes_projects_Achivements",
                table: "dialCodes",
                column: "Achivements",
                principalTable: "projects",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_projects_Projectid",
                table: "User",
                column: "Projectid",
                principalTable: "projects",
                principalColumn: "id");
        }
    }
}
