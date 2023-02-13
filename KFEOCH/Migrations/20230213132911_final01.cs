using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class final01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasForm = table.Column<bool>(type: "bit", nullable: true),
                    FormUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearlyFees = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeLegalEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeLegalEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeOwnerSpecialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeOwnerSpecialities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocal = table.Column<bool>(type: "bit", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OwnerDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasForm = table.Column<bool>(type: "bit", nullable: true),
                    FormUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OwnerPositionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerPositionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEnglish = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubtitleArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubtitleEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Template = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HostUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Governorates_Countries_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_OfficeTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialities_OfficeTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEnglish = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubtitleArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubtitleEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_PostCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "PostCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_Governorates_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEnglish = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubtitleArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubtitleEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    LicenseId = table.Column<long>(type: "bigint", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstablishmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MembershipEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailBox = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaId = table.Column<int>(type: "int", nullable: true),
                    GovernorateId = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    LegalEntityId = table.Column<int>(type: "int", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AgreeToTerms = table.Column<bool>(type: "bit", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInHome = table.Column<bool>(type: "bit", nullable: true),
                    AutoNumberOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutoNumberTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocal = table.Column<bool>(type: "bit", nullable: true),
                    RenewYears = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offices_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offices_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offices_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offices_OfficeEntities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "OfficeEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offices_OfficeLegalEntities_LegalEntityId",
                        column: x => x.LegalEntityId,
                        principalTable: "OfficeLegalEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offices_OfficeTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LicenseId = table.Column<long>(type: "bigint", nullable: false),
                    OfficeTypeId = table.Column<int>(type: "int", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: true),
                    IsPasswordChanged = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_OfficeTypes_OfficeTypeId",
                        column: x => x.OfficeTypeId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    OfficeEntityId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsRejected = table.Column<bool>(type: "bit", nullable: false),
                    IsFirst = table.Column<bool>(type: "bit", nullable: false),
                    IsPending = table.Column<bool>(type: "bit", nullable: false),
                    IsLast = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licenses_OfficeEntities_OfficeEntityId",
                        column: x => x.OfficeEntityId,
                        principalTable: "OfficeEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Licenses_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfficeActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: true),
                    ActivityId = table.Column<int>(type: "int", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeActivities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfficeActivities_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OfficeContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: true),
                    ContactId = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeContacts_ContactTypes_ContactId",
                        column: x => x.ContactId,
                        principalTable: "ContactTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfficeContacts_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OfficeDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeDocuments_OfficeDocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "OfficeDocumentTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfficeDocuments_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OfficeOwners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalityId = table.Column<int>(type: "int", nullable: true),
                    SemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialityId = table.Column<int>(type: "int", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExperienceYears = table.Column<int>(type: "int", nullable: true),
                    SignatureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CvUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeOwners_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeOwners_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Nationalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfficeOwners_OfficeOwnerSpecialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "OfficeOwnerSpecialities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfficeOwners_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeOwners_OwnerPositionTypes_PositionId",
                        column: x => x.PositionId,
                        principalTable: "OwnerPositionTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OfficePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    RequestNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestNameEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    YearsCount = table.Column<double>(type: "float", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentCategoryArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentCategoryEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembershipEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HtmlBody = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficePayments_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficePayments_PaymentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfficeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    RequestTypeId = table.Column<int>(type: "int", nullable: false),
                    CertificateEntityId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CanceledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DoneDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsRejected = table.Column<bool>(type: "bit", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    HtmlBody = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeRequests_CertificateEntities_CertificateEntityId",
                        column: x => x.CertificateEntityId,
                        principalTable: "CertificateEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfficeRequests_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeRequests_RequestTypes_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfficeSpecialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: true),
                    SpecialityId = table.Column<int>(type: "int", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeSpecialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeSpecialities_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfficeSpecialities_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => new { x.ApplicationUserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenseSpeciality",
                columns: table => new
                {
                    LicensesId = table.Column<int>(type: "int", nullable: false),
                    SpecialitiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseSpeciality", x => new { x.LicensesId, x.SpecialitiesId });
                    table.ForeignKey(
                        name: "FK_LicenseSpeciality_Licenses_LicensesId",
                        column: x => x.LicensesId,
                        principalTable: "Licenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LicenseSpeciality_Specialities_SpecialitiesId",
                        column: x => x.SpecialitiesId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OwnerDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnerDocuments_OfficeOwners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "OfficeOwners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OwnerDocuments_OwnerDocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "OwnerDocumentTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_NameArabic_ParentId",
                table: "Activities",
                columns: new[] { "NameArabic", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_NameEnglish_ParentId",
                table: "Activities",
                columns: new[] { "NameEnglish", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ParentId",
                table: "Activities",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_NameArabic_ParentId",
                table: "Areas",
                columns: new[] { "NameArabic", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_NameEnglish_ParentId",
                table: "Areas",
                columns: new[] { "NameEnglish", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ParentId",
                table: "Areas",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                name: "IX_AspNetUsers_LicenseId_OfficeTypeId",
                table: "AspNetUsers",
                columns: new[] { "LicenseId", "OfficeTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OfficeId",
                table: "AspNetUsers",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OfficeTypeId",
                table: "AspNetUsers",
                column: "OfficeTypeId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateEntities_NameArabic",
                table: "CertificateEntities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CertificateEntities_NameEnglish",
                table: "CertificateEntities",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactTypes_NameArabic",
                table: "ContactTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactTypes_NameEnglish",
                table: "ContactTypes",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameArabic",
                table: "Countries",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameEnglish",
                table: "Countries",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseCategories_NameArabic",
                table: "CourseCategories",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseCategories_NameEnglish",
                table: "CourseCategories",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genders_NameArabic",
                table: "Genders",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genders_NameEnglish",
                table: "Genders",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_NameArabic_ParentId",
                table: "Governorates",
                columns: new[] { "NameArabic", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_NameEnglish_ParentId",
                table: "Governorates",
                columns: new[] { "NameEnglish", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_ParentId",
                table: "Governorates",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_OfficeEntityId",
                table: "Licenses",
                column: "OfficeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_OfficeId",
                table: "Licenses",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseSpeciality_SpecialitiesId",
                table: "LicenseSpeciality",
                column: "SpecialitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_NameArabic",
                table: "Nationalities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_NameEnglish",
                table: "Nationalities",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeActivities_ActivityId",
                table: "OfficeActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeActivities_OfficeId_ActivityId",
                table: "OfficeActivities",
                columns: new[] { "OfficeId", "ActivityId" },
                unique: true,
                filter: "[OfficeId] IS NOT NULL AND [ActivityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeContacts_ContactId",
                table: "OfficeContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeContacts_OfficeId",
                table: "OfficeContacts",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeDocuments_OfficeId",
                table: "OfficeDocuments",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeDocuments_TypeId",
                table: "OfficeDocuments",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeDocumentTypes_NameArabic",
                table: "OfficeDocumentTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeDocumentTypes_NameEnglish",
                table: "OfficeDocumentTypes",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeEntities_NameArabic",
                table: "OfficeEntities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeEntities_NameEnglish",
                table: "OfficeEntities",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLegalEntities_NameArabic",
                table: "OfficeLegalEntities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLegalEntities_NameEnglish",
                table: "OfficeLegalEntities",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeOwners_GenderId",
                table: "OfficeOwners",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeOwners_NationalityId",
                table: "OfficeOwners",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeOwners_OfficeId",
                table: "OfficeOwners",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeOwners_PositionId",
                table: "OfficeOwners",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeOwners_SpecialityId",
                table: "OfficeOwners",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeOwnerSpecialities_NameArabic",
                table: "OfficeOwnerSpecialities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeOwnerSpecialities_NameEnglish",
                table: "OfficeOwnerSpecialities",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficePayments_OfficeId",
                table: "OfficePayments",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficePayments_TypeId",
                table: "OfficePayments",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeRequests_CertificateEntityId",
                table: "OfficeRequests",
                column: "CertificateEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeRequests_OfficeId",
                table: "OfficeRequests",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeRequests_RequestTypeId",
                table: "OfficeRequests",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_AreaId",
                table: "Offices",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_CountryId",
                table: "Offices",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_Email",
                table: "Offices",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_EntityId",
                table: "Offices",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_GovernorateId",
                table: "Offices",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_LegalEntityId",
                table: "Offices",
                column: "LegalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_LicenseId_TypeId",
                table: "Offices",
                columns: new[] { "LicenseId", "TypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_NameArabic",
                table: "Offices",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_NameEnglish",
                table: "Offices",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_TypeId",
                table: "Offices",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeSpecialities_OfficeId_SpecialityId",
                table: "OfficeSpecialities",
                columns: new[] { "OfficeId", "SpecialityId" },
                unique: true,
                filter: "[OfficeId] IS NOT NULL AND [SpecialityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeSpecialities_SpecialityId",
                table: "OfficeSpecialities",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeTypes_NameArabic",
                table: "OfficeTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeTypes_NameEnglish",
                table: "OfficeTypes",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDocuments_OwnerId",
                table: "OwnerDocuments",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDocuments_TypeId",
                table: "OwnerDocuments",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDocumentTypes_NameArabic",
                table: "OwnerDocumentTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDocumentTypes_NameEnglish",
                table: "OwnerDocumentTypes",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnerPositionTypes_NameArabic",
                table: "OwnerPositionTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnerPositionTypes_NameEnglish",
                table: "OwnerPositionTypes",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_HostUrl",
                table: "Pages",
                column: "HostUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_NameArabic",
                table: "PaymentTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_NameEnglish",
                table: "PaymentTypes",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCategories_NameArabic",
                table: "PostCategories",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCategories_NameEnglish",
                table: "PostCategories",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PageId",
                table: "Posts",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestTypes_NameArabic",
                table: "RequestTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestTypes_NameEnglish",
                table: "RequestTypes",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_PostId",
                table: "Sections",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_NameArabic_ParentId",
                table: "Specialities",
                columns: new[] { "NameArabic", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_NameEnglish_ParentId",
                table: "Specialities",
                columns: new[] { "NameEnglish", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_ParentId",
                table: "Specialities",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_NameArabic",
                table: "Statuses",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_NameEnglish",
                table: "Statuses",
                column: "NameEnglish",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "CourseCategories");

            migrationBuilder.DropTable(
                name: "LicenseSpeciality");

            migrationBuilder.DropTable(
                name: "OfficeActivities");

            migrationBuilder.DropTable(
                name: "OfficeContacts");

            migrationBuilder.DropTable(
                name: "OfficeDocuments");

            migrationBuilder.DropTable(
                name: "OfficePayments");

            migrationBuilder.DropTable(
                name: "OfficeRequests");

            migrationBuilder.DropTable(
                name: "OfficeSpecialities");

            migrationBuilder.DropTable(
                name: "OwnerDocuments");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "SiteMessages");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "ContactTypes");

            migrationBuilder.DropTable(
                name: "OfficeDocumentTypes");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "CertificateEntities");

            migrationBuilder.DropTable(
                name: "RequestTypes");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.DropTable(
                name: "OfficeOwners");

            migrationBuilder.DropTable(
                name: "OwnerDocumentTypes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "OfficeOwnerSpecialities");

            migrationBuilder.DropTable(
                name: "OwnerPositionTypes");

            migrationBuilder.DropTable(
                name: "Offices");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "PostCategories");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "OfficeEntities");

            migrationBuilder.DropTable(
                name: "OfficeLegalEntities");

            migrationBuilder.DropTable(
                name: "OfficeTypes");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
