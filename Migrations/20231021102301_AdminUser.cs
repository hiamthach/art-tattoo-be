using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class AdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Address", "Avatar", "Birthday", "Email", "FullName", "LastLoginAt", "Password", "Phone", "RoleId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), null, null, null, "admin@arttattoo.com", "Admin Art Tattoo Lover", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$/Tu3pB8wcxurEoqBeXsaluOBoI7jn9T4la9uWRWbcB2Ci3vNyTyj2", null, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
