using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidenceProject.Migrations
{
    /// <inheritdoc />
    public partial class schoolAuthUserByteYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dialInfos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dialInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studyField = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    schoolYear = table.Column<byte>(type: "tinyint", nullable: true),
                    contactDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Applicants = table.Column<int>(type: "int", nullable: true),
                    Assigness = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    globalAdmin = table.Column<bool>(type: "bit", nullable: true),
                    idkey = table.Column<string>(name: "id_key", type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dialCodes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    color = table.Column<int>(name: "_color", type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dialInfoid = table.Column<int>(type: "int", nullable: false),
                    Technology = table.Column<int>(type: "int", nullable: true)
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
                    AuthUser = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    projectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    github = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    slack = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.id);
                    table.ForeignKey(
                        name: "FK_projects_User_AuthUser",
                        column: x => x.AuthUser,
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_projects_dialCodes_State",
                        column: x => x.State,
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
                name: "Achievement",
                columns: table => new
                {
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    projectid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievement", x => x.name);
                    table.ForeignKey(
                        name: "FK_Achievement_projects_projectid",
                        column: x => x.projectid,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    mimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Projectid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_files_projects_Projectid",
                        column: x => x.Projectid,
                        principalTable: "projects",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    Assigness = table.Column<int>(type: "int", nullable: false),
                    assigneesid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.Assigness, x.assigneesid });
                    table.ForeignKey(
                        name: "FK_ProjectUser_User_assigneesid",
                        column: x => x.assigneesid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_projects_Assigness",
                        column: x => x.Assigness,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Achievement_projectid",
                table: "Achievement",
                column: "projectid");

            migrationBuilder.CreateIndex(
                name: "IX_DialCodeProject_projectTechnologyid",
                table: "DialCodeProject",
                column: "projectTechnologyid");

            migrationBuilder.CreateIndex(
                name: "IX_dialCodes_dialInfoid",
                table: "dialCodes",
                column: "dialInfoid");

            migrationBuilder.CreateIndex(
                name: "IX_dialCodes_name",
                table: "dialCodes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dialInfos_name",
                table: "dialInfos",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_files_Projectid",
                table: "files",
                column: "Projectid");

            migrationBuilder.CreateIndex(
                name: "IX_projects_AuthUser",
                table: "projects",
                column: "AuthUser");

            migrationBuilder.CreateIndex(
                name: "IX_projects_State",
                table: "projects",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_projects_Type",
                table: "projects",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_assigneesid",
                table: "ProjectUser",
                column: "assigneesid");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser1_applicantsid",
                table: "ProjectUser1",
                column: "applicantsid");

            migrationBuilder.CreateIndex(
                name: "IX_User_id_key",
                table: "User",
                column: "id_key",
                unique: true,
                filter: "[id_key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_username",
                table: "User",
                column: "username",
                unique: true,
                filter: "[username] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achievement");

            migrationBuilder.DropTable(
                name: "DialCodeProject");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "ProjectUser");

            migrationBuilder.DropTable(
                name: "ProjectUser1");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "dialCodes");

            migrationBuilder.DropTable(
                name: "dialInfos");
        }
    }
}
