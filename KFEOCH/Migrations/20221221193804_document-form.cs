using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class documentform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormUrl",
                table: "OwnerDocumentTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasForm",
                table: "OwnerDocumentTypes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormUrl",
                table: "OfficeDocumentTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasForm",
                table: "OfficeDocumentTypes",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormUrl",
                table: "OwnerDocumentTypes");

            migrationBuilder.DropColumn(
                name: "HasForm",
                table: "OwnerDocumentTypes");

            migrationBuilder.DropColumn(
                name: "FormUrl",
                table: "OfficeDocumentTypes");

            migrationBuilder.DropColumn(
                name: "HasForm",
                table: "OfficeDocumentTypes");
        }
    }
}
