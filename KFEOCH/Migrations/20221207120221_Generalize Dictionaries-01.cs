using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class GeneralizeDictionaries01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_OfficeTypes_OfficeTypeId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Governorates_GovernorateId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Governorates_Countries_CountryId",
                table: "Governorates");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Governorates",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Governorates_CountryId",
                table: "Governorates",
                newName: "IX_Governorates_ParentId");

            migrationBuilder.RenameColumn(
                name: "GovernorateId",
                table: "Areas",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_GovernorateId",
                table: "Areas",
                newName: "IX_Areas_ParentId");

            migrationBuilder.RenameColumn(
                name: "OfficeTypeId",
                table: "Activities",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_OfficeTypeId",
                table: "Activities",
                newName: "IX_Activities_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_OfficeTypes_ParentId",
                table: "Activities",
                column: "ParentId",
                principalTable: "OfficeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Governorates_ParentId",
                table: "Areas",
                column: "ParentId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Governorates_Countries_ParentId",
                table: "Governorates",
                column: "ParentId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_OfficeTypes_ParentId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Governorates_ParentId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Governorates_Countries_ParentId",
                table: "Governorates");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Governorates",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Governorates_ParentId",
                table: "Governorates",
                newName: "IX_Governorates_CountryId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Areas",
                newName: "GovernorateId");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_ParentId",
                table: "Areas",
                newName: "IX_Areas_GovernorateId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Activities",
                newName: "OfficeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ParentId",
                table: "Activities",
                newName: "IX_Activities_OfficeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_OfficeTypes_OfficeTypeId",
                table: "Activities",
                column: "OfficeTypeId",
                principalTable: "OfficeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Governorates_GovernorateId",
                table: "Areas",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Governorates_Countries_CountryId",
                table: "Governorates",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
