using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class AdminUserDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "Password", "Status" },
                values: new object[] { "$2a$11$GaFp3nzQ0PUiSxYs4kE8wunyMIGMizKhgcZOTFHKekhklSzpwumV.", "Active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "Password", "Status" },
                values: new object[] { "$2a$11$/Tu3pB8wcxurEoqBeXsaluOBoI7jn9T4la9uWRWbcB2Ci3vNyTyj2", "Inactive" });
        }
    }
}
