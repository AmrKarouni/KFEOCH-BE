using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class office05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Offices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Offices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_CountryId",
                table: "Offices",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_GovernorateId",
                table: "Offices",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Countries_CountryId",
                table: "Offices",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Governorates_GovernorateId",
                table: "Offices",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Countries_CountryId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Governorates_GovernorateId",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_CountryId",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_GovernorateId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Offices");
        }
    }
}
