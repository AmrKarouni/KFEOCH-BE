using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class DocumentsDictionaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeDocumentTypes", x => x.Id);
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
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerDocumentTypes", x => x.Id);
                });

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
                name: "IX_OwnerDocumentTypes_NameArabic",
                table: "OwnerDocumentTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDocumentTypes_NameEnglish",
                table: "OwnerDocumentTypes",
                column: "NameEnglish",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficeDocumentTypes");

            migrationBuilder.DropTable(
                name: "OwnerDocumentTypes");
        }
    }
}
