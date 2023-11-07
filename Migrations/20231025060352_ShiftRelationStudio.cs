using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class ShiftRelationStudio : Migration
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
                values: new object[] { "$2a$11$RFy.rvqzUJJafZsC61Xzju4aGqE3sYYI4Pa7zVrB9OnM7qxfL8hKq", "Active" });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_StudioId",
                table: "Shifts",
                column: "StudioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_studios_StudioId",
                table: "Shifts",
                column: "StudioId",
                principalTable: "studios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_studios_StudioId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_StudioId",
                table: "Shifts");

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
                values: new object[] { "$2a$11$cdrUz7Sfd9u32V9MjKqWt.UskkTK8K.A7hiD9xvU8PVEcujT38vny", "Inactive" });
        }
    }
}
