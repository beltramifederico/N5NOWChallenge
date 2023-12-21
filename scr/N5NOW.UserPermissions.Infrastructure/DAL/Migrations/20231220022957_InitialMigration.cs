using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N5NOW.UserPermissions.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "permissiontypes",
                schema: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissiontypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeeforename = table.Column<string>(type: "text", nullable: false),
                    employeesurname = table.Column<string>(type: "text", nullable: false),
                    permissiontype = table.Column<int>(type: "integer", nullable: false),
                    permissiondate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_permissions_permissiontypes_permissiontype",
                        column: x => x.permissiontype,
                        principalSchema: "Users",
                        principalTable: "permissiontypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_permissions_permissiontype",
                schema: "Users",
                table: "permissions",
                column: "permissiontype",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "permissions",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "permissiontypes",
                schema: "Users");
        }
    }
}
