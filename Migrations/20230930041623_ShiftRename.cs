using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class ShiftRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_schedules_ScheduleId",
                table: "appointments");

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "appointments",
                newName: "ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_ScheduleId",
                table: "appointments",
                newName: "IX_appointments_ShiftId");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "invoices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_studio_users_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "studio_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_invoices_AppointmentId",
                table: "invoices",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ArtistId",
                table: "Shifts",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_Shifts_ShiftId",
                table: "appointments",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_invoices_appointments_AppointmentId",
                table: "invoices",
                column: "AppointmentId",
                principalTable: "appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_Shifts_ShiftId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_invoices_appointments_AppointmentId",
                table: "invoices");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_invoices_AppointmentId",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "invoices");

            migrationBuilder.RenameColumn(
                name: "ShiftId",
                table: "appointments",
                newName: "ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_ShiftId",
                table: "appointments",
                newName: "IX_appointments_ScheduleId");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_schedules_studio_users_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "studio_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_schedules_ArtistId",
                table: "schedules",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_schedules_ScheduleId",
                table: "appointments",
                column: "ScheduleId",
                principalTable: "schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
