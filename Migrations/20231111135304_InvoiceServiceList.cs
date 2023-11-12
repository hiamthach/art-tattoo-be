using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceServiceList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invoices_studio_services_ServiceId",
                table: "invoices");

            migrationBuilder.DropIndex(
                name: "IX_invoices_ServiceId",
                table: "invoices");

            migrationBuilder.CreateTable(
                name: "invoice_services",
                columns: table => new
                {
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_services", x => new { x.ServiceId, x.InvoiceId });
                    table.ForeignKey(
                        name: "FK_invoice_services_invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_invoice_services_studio_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "studio_services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                keyValue: new Guid("00000000-0000-0000-0000-000000000888"),
                column: "Status",
                value: "Active");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000012345"),
                columns: new[] { "Password", "Status" },
                values: new object[] { "$2a$11$bwPXjHD/QnjDxJ2y7C5KbOdo5/XtlJinpNXIAMbNS4qZHj51MoIZq", "Active" });

            migrationBuilder.CreateIndex(
                name: "IX_invoice_services_InvoiceId",
                table: "invoice_services",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoice_services");

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
                values: new object[] { "$2a$11$2gwPR4FXuO7SyJHyONmLAenUL0OHmKNOSLXX5k2jC135D6JKuINiK", "Inactive" });

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
    }
}
