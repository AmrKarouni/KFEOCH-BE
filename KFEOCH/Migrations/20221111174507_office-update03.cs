using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class officeupdate03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LicenseId",
                table: "Offices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Offices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_TypeId",
                table: "Offices",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_OfficeTypes_TypeId",
                table: "Offices",
                column: "TypeId",
                principalTable: "OfficeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_OfficeTypes_TypeId",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_TypeId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "LicenseId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Offices");
        }
    }
}
