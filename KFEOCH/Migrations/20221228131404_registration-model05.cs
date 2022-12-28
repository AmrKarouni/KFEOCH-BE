using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class registrationmodel05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeLicenses_LicenseStatuses_StatusId",
                table: "OfficeLicenses");

            migrationBuilder.DropTable(
                name: "LicenseStatuses");

            migrationBuilder.DropIndex(
                name: "IX_OfficeLicenses_StatusId",
                table: "OfficeLicenses");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "OfficeLicenses");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberTwo",
                table: "OfficeOwners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "OfficeLicenses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "OfficeLicenses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "OfficeLicenses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "OfficeContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "LicenseRequestType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromEntityId = table.Column<int>(type: "int", nullable: true),
                    ToEntityId = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<double>(type: "float", nullable: true),
                    FeesAmount = table.Column<double>(type: "float", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseRequestType");

            migrationBuilder.DropColumn(
                name: "PhoneNumberTwo",
                table: "OfficeOwners");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "OfficeLicenses");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "OfficeLicenses");

            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "OfficeLicenses");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "OfficeContacts");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "OfficeLicenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LicenseStatuses",
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
                    table.PrimaryKey("PK_LicenseStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLicenses_StatusId",
                table: "OfficeLicenses",
                column: "StatusId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeLicenses_LicenseStatuses_StatusId",
                table: "OfficeLicenses",
                column: "StatusId",
                principalTable: "LicenseStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
