using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class StudioUserMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_studio_users_UserId",
                table: "studio_users");

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
                values: new object[] { "$2a$11$jesHlHj8SfMmzB6ZcGLXPeHaDsS.8D9/k8hK128fhRs7J1J56/Yca", "Active" });

            migrationBuilder.CreateIndex(
                name: "IX_studio_users_UserId",
                table: "studio_users",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_studio_users_UserId",
                table: "studio_users");

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
                values: new object[] { "$2a$11$rjz09huXZkmzGF532vUXv.oVu3VQTaA8Ate.lmC/eTfOI2wtRmGfy", "Inactive" });

            migrationBuilder.CreateIndex(
                name: "IX_studio_users_UserId",
                table: "studio_users",
                column: "UserId",
                unique: true);
        }
    }
}
