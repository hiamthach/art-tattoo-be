using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class AppointmentStudio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_studios_StudioId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_StudioId",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "StudioId",
                table: "appointments");

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
                values: new object[] { "$2a$11$0R4/Q5jYvnACfMVCv8Yh7eoWZtlJT89yDXZpsYhblO54xRd.whjb.", "Active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudioId",
                table: "appointments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                values: new object[] { "$2a$11$RGw/9uI2qf.mYrES5S38Ye.MdIhrJUA6CGzaziSgbDZXcjTS3fKgC", "Inactive" });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_StudioId",
                table: "appointments",
                column: "StudioId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_studios_StudioId",
                table: "appointments",
                column: "StudioId",
                principalTable: "studios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
