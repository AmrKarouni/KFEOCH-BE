using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class registrationmodel18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficePayments_OfficeRequests_RequestId",
                table: "OfficePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficePayments_PaymentTypes_TypeId",
                table: "OfficePayments");

            migrationBuilder.DropIndex(
                name: "IX_OfficePayments_TypeId",
                table: "OfficePayments");

            migrationBuilder.DropColumn(
                name: "YearlyFees",
                table: "OfficeTypes");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "OfficePayments",
                newName: "OfficeId");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "OfficePayments",
                newName: "RequestNameEnglish");

            migrationBuilder.RenameIndex(
                name: "IX_OfficePayments_RequestId",
                table: "OfficePayments",
                newName: "IX_OfficePayments_OfficeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipEndDate",
                table: "Offices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentNumber",
                table: "OfficePayments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestNameArabic",
                table: "OfficePayments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "YearlyFees",
                table: "OfficeEntities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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
                    IsFirst = table.Column<bool>(type: "bit", nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseSpeciality_Specialities_SpecialitiesId",
                        column: x => x.SpecialitiesId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_OfficePayments_Offices_OfficeId",
                table: "OfficePayments",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficePayments_Offices_OfficeId",
                table: "OfficePayments");

            migrationBuilder.DropTable(
                name: "LicenseSpeciality");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropColumn(
                name: "MembershipEndDate",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "PaymentNumber",
                table: "OfficePayments");

            migrationBuilder.DropColumn(
                name: "RequestNameArabic",
                table: "OfficePayments");

            migrationBuilder.DropColumn(
                name: "YearlyFees",
                table: "OfficeEntities");

            migrationBuilder.RenameColumn(
                name: "RequestNameEnglish",
                table: "OfficePayments",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "OfficeId",
                table: "OfficePayments",
                newName: "RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_OfficePayments_OfficeId",
                table: "OfficePayments",
                newName: "IX_OfficePayments_RequestId");

            migrationBuilder.AddColumn<double>(
                name: "YearlyFees",
                table: "OfficeTypes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_OfficePayments_TypeId",
                table: "OfficePayments",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficePayments_OfficeRequests_RequestId",
                table: "OfficePayments",
                column: "RequestId",
                principalTable: "OfficeRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficePayments_PaymentTypes_TypeId",
                table: "OfficePayments",
                column: "TypeId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
