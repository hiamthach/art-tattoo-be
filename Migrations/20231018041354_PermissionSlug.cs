using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class PermissionSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "Slug", "Description", "Name" },
                values: new object[,]
                {
                    { "BLOG.ALL", null, "Manage blog" },
                    { "BLOG.OWN", null, "Manage owned blog" },
                    { "CATE.ALL", null, "Manage category" },
                    { "PER.ALL", null, "Manage permission" },
                    { "ROLE.ALL", null, "Manage role" },
                    { "STU_A.ALL", null, "Manage studio artists" },
                    { "STU_A.R", null, "View studio artists" },
                    { "STU_AS.ALL", null, "Manage studio artists schedule" },
                    { "STU_AS.R", null, "View studio artists schedule" },
                    { "STU_B.ALL", null, "Manage studio booking" },
                    { "STU_B.R", null, "View studio booking" },
                    { "STU_I.ALL", null, "Manage studio invoice" },
                    { "STU_I.R", null, "View studio invoice" },
                    { "STU_S.ALL", null, "Manage studio services" },
                    { "STU_S.R", null, "View studio services" },
                    { "STU_U.R", null, "Manage studio customers" },
                    { "STU.ALL", null, "Manage studio" },
                    { "STU.OWN", null, "Manage owned studio" },
                    { "TESTI.ALL", null, "Manage testimonial" },
                    { "TESTI.OWN", null, "Manage owned testimonial" },
                    { "USR_I.R", null, "View owned invoice" },
                    { "USR.ALL", null, "Manage users" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "BLOG.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "BLOG.OWN");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "CATE.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "PER.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "ROLE.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_A.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_A.R");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_AS.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_AS.R");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_B.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_B.R");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_I.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_I.R");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_S.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_S.R");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU_U.R");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "STU.OWN");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "TESTI.ALL");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "TESTI.OWN");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "USR_I.R");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Slug",
                keyValue: "USR.ALL");
        }
    }
}
