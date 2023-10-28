using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class ShiftRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_studio_users_ArtistId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_ArtistId",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "ArtistId",
                table: "Shifts",
                newName: "StudioId");

            migrationBuilder.CreateTable(
                name: "ShiftStudioUser",
                columns: table => new
                {
                    ArtistsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftStudioUser", x => new { x.ArtistsId, x.ShiftsId });
                    table.ForeignKey(
                        name: "FK_ShiftStudioUser_Shifts_ShiftsId",
                        column: x => x.ShiftsId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftStudioUser_studio_users_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "studio_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                values: new object[] { "$2a$11$cdrUz7Sfd9u32V9MjKqWt.UskkTK8K.A7hiD9xvU8PVEcujT38vny", "Active" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftStudioUser_ShiftsId",
                table: "ShiftStudioUser",
                column: "ShiftsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftStudioUser");

            migrationBuilder.RenameColumn(
                name: "StudioId",
                table: "Shifts",
                newName: "ArtistId");

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
                values: new object[] { "$2a$11$jesHlHj8SfMmzB6ZcGLXPeHaDsS.8D9/k8hK128fhRs7J1J56/Yca", "Inactive" });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ArtistId",
                table: "Shifts",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_studio_users_ArtistId",
                table: "Shifts",
                column: "ArtistId",
                principalTable: "studio_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
