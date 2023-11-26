using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class ServiceUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "studios",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectDuration",
                table: "studio_services",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "studio_services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "studio_services",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "appointments",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "appointments",
                type: "uniqueidentifier",
                nullable: true);

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
                values: new object[] { "$2a$11$4xSnrTY7Fd5rL2PNSqfIJ.XVGo5CDgiOrAuDtUUJ4cZS2m50K6Zju", "Active" });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_ServiceId",
                table: "appointments",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_studio_services_ServiceId",
                table: "appointments",
                column: "ServiceId",
                principalTable: "studio_services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_studio_services_ServiceId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_ServiceId",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "ExpectDuration",
                table: "studio_services");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "studio_services");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "studio_services");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "appointments");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "studios",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

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
                values: new object[] { "$2a$11$0R4/Q5jYvnACfMVCv8Yh7eoWZtlJT89yDXZpsYhblO54xRd.whjb.", "Inactive" });
        }
    }
}
