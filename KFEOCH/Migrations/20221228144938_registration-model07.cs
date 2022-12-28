using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class registrationmodel07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseRequestType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseRequestType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Duration = table.Column<double>(type: "float", nullable: true),
                    FeesAmount = table.Column<double>(type: "float", nullable: true),
                    FromEntityId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ToEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRequestType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRequestType_NameArabic",
                table: "LicenseRequestType",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRequestType_NameEnglish",
                table: "LicenseRequestType",
                column: "NameEnglish",
                unique: true);
        }
    }
}
