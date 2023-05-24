using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoApplication.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "e1557903-e1f2-4cba-b28e-291553145163");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b7a5fefb-2d2d-4caa-ad13-e1064e3cc12c", "AQAAAAEAACcQAAAAEKmR7ac1lB09dm4zkvEe2kfwvfhxkT572/i2jlvTc6FR4CGrW7CeHDQXK5CXkbeM0A==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "64141eef-1a0f-41db-8207-234691ed2268");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "85c4df4d-4bce-41f3-894b-049c5f5352a5", "AQAAAAEAACcQAAAAEM/16WFKCp9aAXW8oxXpRUKT3hS0PmHFfQvJs/1H3e/41woN2/XwPID4yXTFB5FjEQ==" });
        }
    }
}
