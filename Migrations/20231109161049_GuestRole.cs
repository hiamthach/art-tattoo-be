using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class GuestRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000404"),
                column: "Status",
                value: "Active");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000012345"),
                columns: new[] { "Password", "Status" },
                values: new object[] { "$2a$11$.OtBLDtL1WKWQZ7sKkrUlOkUj0I5nLNacOqxBPkhCmiMa51zDcb3q", "Active" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Address", "Avatar", "Birthday", "Email", "FullName", "LastLoginAt", "Password", "Phone", "RoleId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000666"), null, null, null, "", "Guest", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 6 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000666"));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000404"),
                column: "Status",
                value: "Inactive");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000012345"),
                columns: new[] { "Password", "Status" },
                values: new object[] { "$2a$11$4xSnrTY7Fd5rL2PNSqfIJ.XVGo5CDgiOrAuDtUUJ4cZS2m50K6Zju", "Inactive" });
        }
    }
}
