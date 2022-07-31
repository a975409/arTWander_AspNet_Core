using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace arTWander.Migrations
{
    public partial class UserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AccessFailedCount", "Birthday", "Email", "EmailConfirmed", "LastLoginDateTime", "LockoutEnabled", "LockoutEndDateTime", "Name", "Password", "PhoneNumber", "Picture", "RoleId", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, null, null, "a975409@gmail.com", null, null, null, null, null, "acs856745", null, null, 1, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
