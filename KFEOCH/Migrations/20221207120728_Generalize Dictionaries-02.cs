using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class GeneralizeDictionaries02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Governorates_NameArabic",
                table: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Governorates_NameEnglish",
                table: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Areas_NameArabic",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_NameEnglish",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Activities_NameArabic",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_NameEnglish",
                table: "Activities");

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_NameArabic_ParentId",
                table: "Governorates",
                columns: new[] { "NameArabic", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_NameEnglish_ParentId",
                table: "Governorates",
                columns: new[] { "NameEnglish", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_NameArabic_ParentId",
                table: "Areas",
                columns: new[] { "NameArabic", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_NameEnglish_ParentId",
                table: "Areas",
                columns: new[] { "NameEnglish", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_NameArabic_ParentId",
                table: "Activities",
                columns: new[] { "NameArabic", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_NameEnglish_ParentId",
                table: "Activities",
                columns: new[] { "NameEnglish", "ParentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Governorates_NameArabic_ParentId",
                table: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Governorates_NameEnglish_ParentId",
                table: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Areas_NameArabic_ParentId",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_NameEnglish_ParentId",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Activities_NameArabic_ParentId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_NameEnglish_ParentId",
                table: "Activities");

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_NameArabic",
                table: "Governorates",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_NameEnglish",
                table: "Governorates",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_NameArabic",
                table: "Areas",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_NameEnglish",
                table: "Areas",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_NameArabic",
                table: "Activities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_NameEnglish",
                table: "Activities",
                column: "NameEnglish",
                unique: true);
        }
    }
}
