using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidenceProject.Migrations
{
    public partial class FixDialCons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "dialInfos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "dialCodes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "_color",
                table: "dialCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_dialInfos_name",
                table: "dialInfos",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dialCodes_name",
                table: "dialCodes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_projectid",
                table: "Achievement",
                column: "projectid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achievement");

            migrationBuilder.DropIndex(
                name: "IX_dialInfos_name",
                table: "dialInfos");

            migrationBuilder.DropIndex(
                name: "IX_dialCodes_name",
                table: "dialCodes");

            migrationBuilder.DropColumn(
                name: "_color",
                table: "dialCodes");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "dialInfos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "dialCodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
