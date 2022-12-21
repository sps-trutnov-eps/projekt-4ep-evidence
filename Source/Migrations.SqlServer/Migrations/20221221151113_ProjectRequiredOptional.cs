using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidenceProject.Migrations
{
    /// <inheritdoc />
    public partial class ProjectRequiredOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_users_AuthUser",
                table: "projects");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AuthUser",
                table: "projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_projects_users_AuthUser",
                table: "projects",
                column: "AuthUser",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_users_AuthUser",
                table: "projects");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthUser",
                table: "projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_users_AuthUser",
                table: "projects",
                column: "AuthUser",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
