using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Migrations.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class PostgresMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dialInfos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    desc = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dialInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fullName = table.Column<string>(type: "text", nullable: false),
                    studyField = table.Column<string>(type: "text", nullable: true),
                    schoolYear = table.Column<byte>(type: "smallint", nullable: true),
                    contactDetails = table.Column<string>(type: "text", nullable: true),
                    Applicants = table.Column<int>(type: "integer", nullable: true),
                    Assigness = table.Column<int>(type: "integer", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    globalAdmin = table.Column<bool>(type: "boolean", nullable: true),
                    idkey = table.Column<string>(name: "id_key", type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dialCodes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<int>(name: "_color", type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    dialInfoid = table.Column<int>(type: "integer", nullable: false),
                    Technology = table.Column<int>(type: "integer", nullable: true)
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
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthUser = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    projectDescription = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    github = table.Column<string>(type: "text", nullable: true),
                    slack = table.Column<string>(type: "text", nullable: true)
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
                        name: "FK_projects_dialCodes_Type",
                        column: x => x.Type,
                        principalTable: "dialCodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_projects_users_AuthUser",
                        column: x => x.AuthUser,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Achievement",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    projectid = table.Column<int>(type: "integer", nullable: false)
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
                    Technology = table.Column<int>(type: "integer", nullable: false),
                    projectTechnologyid = table.Column<int>(type: "integer", nullable: false)
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
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    originalFileName = table.Column<string>(type: "text", nullable: false),
                    generatedFileName = table.Column<string>(type: "text", nullable: false),
                    fileData = table.Column<byte[]>(type: "bytea", nullable: false),
                    mimeType = table.Column<string>(type: "text", nullable: false),
                    Projectid = table.Column<int>(type: "integer", nullable: true)
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
                    Assigness = table.Column<int>(type: "integer", nullable: false),
                    assigneesid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.Assigness, x.assigneesid });
                    table.ForeignKey(
                        name: "FK_ProjectUser_projects_Assigness",
                        column: x => x.Assigness,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_users_assigneesid",
                        column: x => x.assigneesid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUser1",
                columns: table => new
                {
                    Applicants = table.Column<int>(type: "integer", nullable: false),
                    applicantsid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser1", x => new { x.Applicants, x.applicantsid });
                    table.ForeignKey(
                        name: "FK_ProjectUser1_projects_Applicants",
                        column: x => x.Applicants,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser1_users_applicantsid",
                        column: x => x.applicantsid,
                        principalTable: "users",
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
                name: "IX_users_id_key",
                table: "users",
                column: "id_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
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
                name: "dialCodes");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "dialInfos");
        }
    }
}
