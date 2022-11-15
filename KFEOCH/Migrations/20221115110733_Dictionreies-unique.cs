using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class Dictionreiesunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "OfficeTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "OfficeTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "officeStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "officeStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "OfficeSpecialities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "OfficeSpecialities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "OfficeLegalEntities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "OfficeLegalEntities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "OfficeEntities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "OfficeEntities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "OfficeActivities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "OfficeActivities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "Governorates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "Governorates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "Genders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "Genders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "CourseCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "CourseCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "Countries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "Countries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "CertificateEntities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "CertificateEntities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "Areas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "Areas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeTypes_NameArabic",
                table: "OfficeTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeTypes_NameEnglish",
                table: "OfficeTypes",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_officeStatuses_NameArabic",
                table: "officeStatuses",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_officeStatuses_NameEnglish",
                table: "officeStatuses",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeSpecialities_NameArabic",
                table: "OfficeSpecialities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeSpecialities_NameEnglish",
                table: "OfficeSpecialities",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLegalEntities_NameArabic",
                table: "OfficeLegalEntities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLegalEntities_NameEnglish",
                table: "OfficeLegalEntities",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeEntities_NameArabic",
                table: "OfficeEntities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeEntities_NameEnglish",
                table: "OfficeEntities",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeActivities_NameArabic",
                table: "OfficeActivities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeActivities_NameEnglish",
                table: "OfficeActivities",
                column: "NameEnglish",
                unique: true);

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
                name: "IX_Genders_NameArabic",
                table: "Genders",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genders_NameEnglish",
                table: "Genders",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseCategories_NameArabic",
                table: "CourseCategories",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseCategories_NameEnglish",
                table: "CourseCategories",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameArabic",
                table: "Countries",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameEnglish",
                table: "Countries",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CertificateEntities_NameArabic",
                table: "CertificateEntities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CertificateEntities_NameEnglish",
                table: "CertificateEntities",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OfficeTypes_NameArabic",
                table: "OfficeTypes");

            migrationBuilder.DropIndex(
                name: "IX_OfficeTypes_NameEnglish",
                table: "OfficeTypes");

            migrationBuilder.DropIndex(
                name: "IX_officeStatuses_NameArabic",
                table: "officeStatuses");

            migrationBuilder.DropIndex(
                name: "IX_officeStatuses_NameEnglish",
                table: "officeStatuses");

            migrationBuilder.DropIndex(
                name: "IX_OfficeSpecialities_NameArabic",
                table: "OfficeSpecialities");

            migrationBuilder.DropIndex(
                name: "IX_OfficeSpecialities_NameEnglish",
                table: "OfficeSpecialities");

            migrationBuilder.DropIndex(
                name: "IX_OfficeLegalEntities_NameArabic",
                table: "OfficeLegalEntities");

            migrationBuilder.DropIndex(
                name: "IX_OfficeLegalEntities_NameEnglish",
                table: "OfficeLegalEntities");

            migrationBuilder.DropIndex(
                name: "IX_OfficeEntities_NameArabic",
                table: "OfficeEntities");

            migrationBuilder.DropIndex(
                name: "IX_OfficeEntities_NameEnglish",
                table: "OfficeEntities");

            migrationBuilder.DropIndex(
                name: "IX_OfficeActivities_NameArabic",
                table: "OfficeActivities");

            migrationBuilder.DropIndex(
                name: "IX_OfficeActivities_NameEnglish",
                table: "OfficeActivities");

            migrationBuilder.DropIndex(
                name: "IX_Governorates_NameArabic",
                table: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Governorates_NameEnglish",
                table: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Genders_NameArabic",
                table: "Genders");

            migrationBuilder.DropIndex(
                name: "IX_Genders_NameEnglish",
                table: "Genders");

            migrationBuilder.DropIndex(
                name: "IX_CourseCategories_NameArabic",
                table: "CourseCategories");

            migrationBuilder.DropIndex(
                name: "IX_CourseCategories_NameEnglish",
                table: "CourseCategories");

            migrationBuilder.DropIndex(
                name: "IX_Countries_NameArabic",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_NameEnglish",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_CertificateEntities_NameArabic",
                table: "CertificateEntities");

            migrationBuilder.DropIndex(
                name: "IX_CertificateEntities_NameEnglish",
                table: "CertificateEntities");

            migrationBuilder.DropIndex(
                name: "IX_Areas_NameArabic",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_NameEnglish",
                table: "Areas");

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "OfficeTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "OfficeTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "officeStatuses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "officeStatuses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "OfficeSpecialities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "OfficeSpecialities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "OfficeLegalEntities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "OfficeLegalEntities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "OfficeEntities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "OfficeEntities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "OfficeActivities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "OfficeActivities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "Governorates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "Governorates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "Genders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "Genders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "CourseCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "CourseCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "CertificateEntities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "CertificateEntities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameEnglish",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameArabic",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
