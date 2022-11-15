using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class Dictionreiesunique04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OfficeSpecialities_NameArabic",
                table: "OfficeSpecialities");

            migrationBuilder.DropIndex(
                name: "IX_OfficeSpecialities_NameEnglish",
                table: "OfficeSpecialities");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeSpecialities_NameArabic_OfficeTypeId",
                table: "OfficeSpecialities",
                columns: new[] { "NameArabic", "OfficeTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeSpecialities_NameEnglish_OfficeTypeId",
                table: "OfficeSpecialities",
                columns: new[] { "NameEnglish", "OfficeTypeId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OfficeSpecialities_NameArabic_OfficeTypeId",
                table: "OfficeSpecialities");

            migrationBuilder.DropIndex(
                name: "IX_OfficeSpecialities_NameEnglish_OfficeTypeId",
                table: "OfficeSpecialities");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeSpecialities_NameArabic",
                table: "OfficeSpecialities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeSpecialities_NameEnglish",
                table: "OfficeSpecialities",
                column: "NameEnglish",
                unique: true);
        }
    }
}
