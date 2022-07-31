using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace arTWander.Migrations
{
    public partial class RoleData1035 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Active", "Name" },
                values: new object[] { 1, true, "admin" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Active", "Name" },
                values: new object[] { 2, true, "common" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2);
        }
    }
}
