﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace art_tattoo_be.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "media",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Url = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.Slug);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "studios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AI"),
                    Slogan = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Introduction = table.Column<string>(type: "longtext", nullable: true),
                    Detail = table.Column<string>(type: "longtext", nullable: true),
                    Logo = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Website = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Facebook = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Instagram = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    Rating = table.Column<double>(type: "double", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studios", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tattoo_categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tattoo_categories", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionsSlug = table.Column<string>(type: "varchar(10)", nullable: false),
                    RolesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsSlug, x.RolesId });
                    table.ForeignKey(
                        name: "FK_PermissionRole_permissions_PermissionsSlug",
                        column: x => x.PermissionsSlug,
                        principalTable: "permissions",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AI"),
                    Phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Avatar = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastLoginAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MediaStudio",
                columns: table => new
                {
                    ListMediaId = table.Column<Guid>(type: "char(36)", nullable: false),
                    StudioMediaId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaStudio", x => new { x.ListMediaId, x.StudioMediaId });
                    table.ForeignKey(
                        name: "FK_MediaStudio_media_ListMediaId",
                        column: x => x.ListMediaId,
                        principalTable: "media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaStudio_studios_StudioMediaId",
                        column: x => x.StudioMediaId,
                        principalTable: "studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    StudioId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_studios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "studio_working_time",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    StudioId = table.Column<Guid>(type: "char(36)", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OpenAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    CloseAt = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studio_working_time", x => x.Id);
                    table.ForeignKey(
                        name: "FK_studio_working_time_studios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "studio_services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    StudioId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AI"),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    MinPrice = table.Column<double>(type: "double", nullable: false),
                    MaxPrice = table.Column<double>(type: "double", nullable: false),
                    Discount = table.Column<double>(type: "double", nullable: false),
                    IsDisabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Thumbnail = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    ExpectDuration = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studio_services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_studio_services_studios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_studio_services_tattoo_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "tattoo_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AI"),
                    Slug = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    IsPublish = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false),
                    StudioId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MediaUser",
                columns: table => new
                {
                    ListMediaId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserMediaId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaUser", x => new { x.ListMediaId, x.UserMediaId });
                    table.ForeignKey(
                        name: "FK_MediaUser_media_ListMediaId",
                        column: x => x.ListMediaId,
                        principalTable: "media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaUser_users_UserMediaId",
                        column: x => x.UserMediaId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "studio_users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    StudioId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studio_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_studio_users_studios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_studio_users_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "testimonials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    StudioId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Rating = table.Column<double>(type: "double", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testimonials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_testimonials_studios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_testimonials_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_testimonials_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MediaStudioService",
                columns: table => new
                {
                    ListMediaId = table.Column<Guid>(type: "char(36)", nullable: false),
                    StudioServiceMediaId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaStudioService", x => new { x.ListMediaId, x.StudioServiceMediaId });
                    table.ForeignKey(
                        name: "FK_MediaStudioService_media_ListMediaId",
                        column: x => x.ListMediaId,
                        principalTable: "media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaStudioService_studio_services_StudioServiceMediaId",
                        column: x => x.StudioServiceMediaId,
                        principalTable: "studio_services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ShiftId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ServiceId = table.Column<Guid>(type: "char(36)", nullable: true),
                    DoneBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_appointments_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_studio_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "studio_services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_studio_users_DoneBy",
                        column: x => x.DoneBy,
                        principalTable: "studio_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointments_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shift_users",
                columns: table => new
                {
                    StuUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ShiftId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IsBooked = table.Column<bool>(type: "tinyint(1)", nullable: false)
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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppointmentMedia",
                columns: table => new
                {
                    AppointmentMediaId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ListMediaId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentMedia", x => new { x.AppointmentMediaId, x.ListMediaId });
                    table.ForeignKey(
                        name: "FK_AppointmentMedia_appointments_AppointmentMediaId",
                        column: x => x.AppointmentMediaId,
                        principalTable: "appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentMedia_media_ListMediaId",
                        column: x => x.ListMediaId,
                        principalTable: "media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    StudioId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Total = table.Column<double>(type: "double", nullable: false),
                    PayMethod = table.Column<string>(type: "longtext", nullable: false),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    AppointmentId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_invoices_appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_invoices_studios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_invoices_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "invoice_services",
                columns: table => new
                {
                    ServiceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false),
                    Discount = table.Column<double>(type: "double", nullable: false)
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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Admin", "ADMIN" },
                    { 2, "System Staff", "SYSTEM_STAFF" },
                    { 3, "Studio Manager", "STUDIO_MANAGER" },
                    { 4, "Studio Staff", "STUDIO_STAFF" },
                    { 5, "Studio Artist", "ARTIST" },
                    { 6, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Address", "Avatar", "Birthday", "Email", "FullName", "LastLoginAt", "Password", "Phone", "RoleId", "Status" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000404"), null, null, null, "", "Deleted User", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 6, "Inactive" },
                    { new Guid("00000000-0000-0000-0000-000000000888"), null, null, null, "guestguest123@guestguest123.com", "Guest", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, 6, "Inactive" },
                    { new Guid("00000000-0000-0000-0000-000000012345"), null, null, null, "arttattoolover@gmail.com", "Admin Art Tattoo Lover", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$ZuYfSO7tEik1gSwOtf.GF.qILqVF1ArUzhL7oA4IUh/1SfCC6KjNe", null, 1, "Inactive" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentMedia_ListMediaId",
                table: "AppointmentMedia",
                column: "ListMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_DoneBy",
                table: "appointments",
                column: "DoneBy");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_ServiceId",
                table: "appointments",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_ShiftId",
                table: "appointments",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_UserId",
                table: "appointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_blogs_CreatedBy",
                table: "blogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_blogs_StudioId",
                table: "blogs",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_services_InvoiceId",
                table: "invoice_services",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_AppointmentId",
                table: "invoices",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_StudioId",
                table: "invoices",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_UserId",
                table: "invoices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaStudio_StudioMediaId",
                table: "MediaStudio",
                column: "StudioMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaStudioService_StudioServiceMediaId",
                table: "MediaStudioService",
                column: "StudioServiceMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaUser_UserMediaId",
                table: "MediaUser",
                column: "UserMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RolesId",
                table: "PermissionRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_Slug",
                table: "permissions",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_Name",
                table: "roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shift_users_ShiftId",
                table: "shift_users",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_StudioId",
                table: "Shifts",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_studio_services_CategoryId",
                table: "studio_services",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_studio_services_StudioId",
                table: "studio_services",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_studio_users_StudioId",
                table: "studio_users",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_studio_users_UserId",
                table: "studio_users",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_studio_working_time_StudioId",
                table: "studio_working_time",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_studios_Name",
                table: "studios",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_testimonials_CreatedBy",
                table: "testimonials",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_testimonials_StudioId",
                table: "testimonials",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_testimonials_UserId",
                table: "testimonials",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentMedia");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "blogs");

            migrationBuilder.DropTable(
                name: "invoice_services");

            migrationBuilder.DropTable(
                name: "MediaStudio");

            migrationBuilder.DropTable(
                name: "MediaStudioService");

            migrationBuilder.DropTable(
                name: "MediaUser");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "shift_users");

            migrationBuilder.DropTable(
                name: "studio_working_time");

            migrationBuilder.DropTable(
                name: "testimonials");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "media");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "studio_services");

            migrationBuilder.DropTable(
                name: "studio_users");

            migrationBuilder.DropTable(
                name: "tattoo_categories");

            migrationBuilder.DropTable(
                name: "studios");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
