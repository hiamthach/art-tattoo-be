using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class UserBirthday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "users");
        }
    }
}
