using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class GeneralizeDictionaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialities_OfficeTypes_OfficeTypeId",
                table: "Specialities");

            migrationBuilder.RenameColumn(
                name: "OfficeTypeId",
                table: "Specialities",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Specialities_OfficeTypeId",
                table: "Specialities",
                newName: "IX_Specialities_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Specialities_NameEnglish_OfficeTypeId",
                table: "Specialities",
                newName: "IX_Specialities_NameEnglish_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Specialities_NameArabic_OfficeTypeId",
                table: "Specialities",
                newName: "IX_Specialities_NameArabic_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialities_OfficeTypes_ParentId",
                table: "Specialities",
                column: "ParentId",
                principalTable: "OfficeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialities_OfficeTypes_ParentId",
                table: "Specialities");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Specialities",
                newName: "OfficeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Specialities_ParentId",
                table: "Specialities",
                newName: "IX_Specialities_OfficeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Specialities_NameEnglish_ParentId",
                table: "Specialities",
                newName: "IX_Specialities_NameEnglish_OfficeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Specialities_NameArabic_ParentId",
                table: "Specialities",
                newName: "IX_Specialities_NameArabic_OfficeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialities_OfficeTypes_OfficeTypeId",
                table: "Specialities",
                column: "OfficeTypeId",
                principalTable: "OfficeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
