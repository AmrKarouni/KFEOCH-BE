using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class registrationmodel04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficeRegistrations");

            migrationBuilder.DropTable(
                name: "RegistrationStatuses");

            migrationBuilder.CreateTable(
                name: "LicenseStatuses",
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
                    table.PrimaryKey("PK_LicenseStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeLicenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentAmount = table.Column<double>(type: "float", nullable: true),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeLicenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeLicenses_LicenseStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "LicenseStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeLicenses_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeLicenses_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseStatuses_NameArabic",
                table: "LicenseStatuses",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseStatuses_NameEnglish",
                table: "LicenseStatuses",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLicenses_OfficeId",
                table: "OfficeLicenses",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLicenses_PaymentTypeId",
                table: "OfficeLicenses",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLicenses_StatusId",
                table: "OfficeLicenses",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficeLicenses");

            migrationBuilder.DropTable(
                name: "LicenseStatuses");

            migrationBuilder.CreateTable(
                name: "RegistrationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true),
                    PaymentAmount = table.Column<double>(type: "float", nullable: true),
                    RegistrationEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    paymentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeRegistrations_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeRegistrations_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeRegistrations_RegistrationStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "RegistrationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfficeRegistrations_OfficeId",
                table: "OfficeRegistrations",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeRegistrations_PaymentTypeId",
                table: "OfficeRegistrations",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeRegistrations_StatusId",
                table: "OfficeRegistrations",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationStatuses_NameArabic",
                table: "RegistrationStatuses",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationStatuses_NameEnglish",
                table: "RegistrationStatuses",
                column: "NameEnglish",
                unique: true);
        }
    }
}
