using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidenceProject.Migrations
{
    /// <inheritdoc />
    public partial class UserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_User_AuthUser",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUser_User_assigneesid",
                table: "ProjectUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUser1_User_applicantsid",
                table: "ProjectUser1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "users");

            migrationBuilder.RenameIndex(
                name: "IX_User_username",
                table: "users",
                newName: "IX_users_username");

            migrationBuilder.RenameIndex(
                name: "IX_User_id_key",
                table: "users",
                newName: "IX_users_id_key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_users_AuthUser",
                table: "projects",
                column: "AuthUser",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUser_users_assigneesid",
                table: "ProjectUser",
                column: "assigneesid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUser1_users_applicantsid",
                table: "ProjectUser1",
                column: "applicantsid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_users_AuthUser",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUser_users_assigneesid",
                table: "ProjectUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUser1_users_applicantsid",
                table: "ProjectUser1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_users_username",
                table: "User",
                newName: "IX_User_username");

            migrationBuilder.RenameIndex(
                name: "IX_users_id_key",
                table: "User",
                newName: "IX_User_id_key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_User_AuthUser",
                table: "projects",
                column: "AuthUser",
                principalTable: "User",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUser_User_assigneesid",
                table: "ProjectUser",
                column: "assigneesid",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUser1_User_applicantsid",
                table: "ProjectUser1",
                column: "applicantsid",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
