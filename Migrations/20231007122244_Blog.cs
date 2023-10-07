using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class Blog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invoices_appointments_AppointmentId",
                table: "invoices");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "studios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Inactive");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "studio_users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_blogs_studios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "studios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_blogs_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blogs_CreatedBy",
                table: "blogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_blogs_StudioId",
                table: "blogs",
                column: "StudioId");

            migrationBuilder.AddForeignKey(
                name: "FK_invoices_appointments_AppointmentId",
                table: "invoices",
                column: "AppointmentId",
                principalTable: "appointments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invoices_appointments_AppointmentId",
                table: "invoices");

            migrationBuilder.DropTable(
                name: "blogs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "studios");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "studio_users");

            migrationBuilder.AddForeignKey(
                name: "FK_invoices_appointments_AppointmentId",
                table: "invoices",
                column: "AppointmentId",
                principalTable: "appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
