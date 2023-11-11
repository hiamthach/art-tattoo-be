using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "invoices",
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
                keyValue: new Guid("00000000-0000-0000-0000-000000000888"),
                column: "Status",
                value: "Active");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000012345"),
                columns: new[] { "Password", "Status" },
                values: new object[] { "$2a$11$2gwPR4FXuO7SyJHyONmLAenUL0OHmKNOSLXX5k2jC135D6JKuINiK", "Active" });

            migrationBuilder.CreateIndex(
                name: "IX_invoices_ServiceId",
                table: "invoices",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_invoices_studio_services_ServiceId",
                table: "invoices",
                column: "ServiceId",
                principalTable: "studio_services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invoices_studio_services_ServiceId",
                table: "invoices");

            migrationBuilder.DropIndex(
                name: "IX_invoices_ServiceId",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "invoices");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000404"),
                column: "Status",
                value: "Inactive");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000888"),
                column: "Status",
                value: "Inactive");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000012345"),
                columns: new[] { "Password", "Status" },
                values: new object[] { "$2a$11$GTEKkpt9CDxMydnm2g.zvOLmTnQFIeuD19md2kT.ZX9hlZkUnQDL.", "Inactive" });
        }
    }
}
