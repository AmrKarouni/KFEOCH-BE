using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class uniqueindx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "Offices",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "Offices",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
                name: "IX_AspNetUsers_LicenseId_OfficeTypeId",
                table: "AspNetUsers",
                columns: new[] { "LicenseId", "OfficeTypeId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Offices_LicenseId_TypeId",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_NameArabic",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_NameEnglish",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LicenseId_OfficeTypeId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
