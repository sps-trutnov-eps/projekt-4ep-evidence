using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidenceProject.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dialInfos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dialInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dialCodes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dialInfoid = table.Column<int>(type: "int", nullable: false),
                    ProjectAchivements = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dialCodes", x => x.id);
                    table.ForeignKey(
                        name: "FK_dialCodes_dialInfos_dialInfoid",
                        column: x => x.dialInfoid,
                        principalTable: "dialInfos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectState = table.Column<int>(type: "int", nullable: false),
                    ProjectType = table.Column<int>(type: "int", nullable: false),
                    ProjectTechnology = table.Column<int>(type: "int", nullable: false),
                    github = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    slack = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.id);
                    table.ForeignKey(
                        name: "FK_projects_dialCodes_ProjectState",
                        column: x => x.ProjectState,
                        principalTable: "dialCodes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_projects_dialCodes_ProjectTechnology",
                        column: x => x.ProjectTechnology,
                        principalTable: "dialCodes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_projects_dialCodes_ProjectType",
                        column: x => x.ProjectType,
                        principalTable: "dialCodes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studyField = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contactDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Projectid = table.Column<int>(type: "int", nullable: true),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    globalAdmin = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_projects_Projectid",
                        column: x => x.Projectid,
                        principalTable: "projects",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_dialCodes_dialInfoid",
                table: "dialCodes",
                column: "dialInfoid");

            migrationBuilder.CreateIndex(
                name: "IX_dialCodes_ProjectAchivements",
                table: "dialCodes",
                column: "ProjectAchivements");

            migrationBuilder.CreateIndex(
                name: "IX_projects_ProjectState",
                table: "projects",
                column: "ProjectState");

            migrationBuilder.CreateIndex(
                name: "IX_projects_ProjectTechnology",
                table: "projects",
                column: "ProjectTechnology");

            migrationBuilder.CreateIndex(
                name: "IX_projects_ProjectType",
                table: "projects",
                column: "ProjectType");

            migrationBuilder.CreateIndex(
                name: "IX_User_Projectid",
                table: "User",
                column: "Projectid");

            migrationBuilder.CreateIndex(
                name: "IX_User_username",
                table: "User",
                column: "username",
                unique: true,
                filter: "[username] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_dialCodes_projects_ProjectAchivements",
                table: "dialCodes",
                column: "ProjectAchivements",
                principalTable: "projects",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dialCodes_dialInfos_dialInfoid",
                table: "dialCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_dialCodes_projects_ProjectAchivements",
                table: "dialCodes");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "dialInfos");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "dialCodes");
        }
    }
}
