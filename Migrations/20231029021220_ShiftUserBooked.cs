using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class ShiftUserBooked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftStudioUser");

            migrationBuilder.CreateTable(
                name: "shift_users",
                columns: table => new
                {
                    StuUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shift_users", x => new { x.StuUserId, x.ShiftId });
                    table.ForeignKey(
                        name: "FK_shift_users_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shift_users_studio_users_StuUserId",
                        column: x => x.StuUserId,
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
                values: new object[] { "$2a$11$RGw/9uI2qf.mYrES5S38Ye.MdIhrJUA6CGzaziSgbDZXcjTS3fKgC", "Active" });

            migrationBuilder.CreateIndex(
                name: "IX_shift_users_ShiftId",
                table: "shift_users",
                column: "ShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shift_users");

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
                value: "Inactive");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000012345"),
                columns: new[] { "Password", "Status" },
                values: new object[] { "$2a$11$RFy.rvqzUJJafZsC61Xzju4aGqE3sYYI4Pa7zVrB9OnM7qxfL8hKq", "Inactive" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftStudioUser_ShiftsId",
                table: "ShiftStudioUser",
                column: "ShiftsId");
        }
    }
}
