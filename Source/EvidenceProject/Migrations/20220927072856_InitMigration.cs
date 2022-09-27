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
                    Achivements = table.Column<int>(type: "int", nullable: true)
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
                    State = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Technology = table.Column<int>(type: "int", nullable: false),
                    github = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    slack = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.id);
                    table.ForeignKey(
                        name: "FK_projects_dialCodes_State",
                        column: x => x.State,
                        principalTable: "dialCodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_projects_dialCodes_Technology",
                        column: x => x.Technology,
                        principalTable: "dialCodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_projects_dialCodes_Type",
                        column: x => x.Type,
                        principalTable: "dialCodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_dialCodes_Achivements",
                table: "dialCodes",
                column: "Achivements");

            migrationBuilder.CreateIndex(
                name: "IX_dialCodes_dialInfoid",
                table: "dialCodes",
                column: "dialInfoid");

            migrationBuilder.CreateIndex(
                name: "IX_projects_State",
                table: "projects",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_projects_Technology",
                table: "projects",
                column: "Technology");

            migrationBuilder.CreateIndex(
                name: "IX_projects_Type",
                table: "projects",
                column: "Type");

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
                name: "FK_dialCodes_projects_Achivements",
                table: "dialCodes",
                column: "Achivements",
                principalTable: "projects",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dialCodes_dialInfos_dialInfoid",
                table: "dialCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_dialCodes_projects_Achivements",
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
