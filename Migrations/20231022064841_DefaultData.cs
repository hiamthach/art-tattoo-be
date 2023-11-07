using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class DefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Address", "Avatar", "Birthday", "Email", "FullName", "LastLoginAt", "Password", "Phone", "RoleId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000404"), null, null, null, "", "Deleted User", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 6 },
                    { new Guid("00000000-0000-0000-0000-000000012345"), null, null, null, "arttattoolover@gmail.com", "Admin Art Tattoo Lover", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$rjz09huXZkmzGF532vUXv.oVu3VQTaA8Ate.lmC/eTfOI2wtRmGfy", null, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000404"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000012345"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Address", "Avatar", "Birthday", "Email", "FullName", "LastLoginAt", "Password", "Phone", "RoleId", "Status" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), null, null, null, "admin@arttattoo.com", "Admin Art Tattoo Lover", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$GaFp3nzQ0PUiSxYs4kE8wunyMIGMizKhgcZOTFHKekhklSzpwumV.", null, 1, "Inactive" });
        }
    }
}
