using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class PostgresMigration2 : Migration
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
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AuthUser",
                table: "projects",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
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
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthUser",
                table: "projects",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_users_AuthUser",
                table: "projects",
                column: "AuthUser",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
