using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class ownerdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "OfficeOwners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NationalityId",
                table: "OfficeOwners",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "OfficeOwners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfficeOwners_NationalityId",
                table: "OfficeOwners",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_NameArabic",
                table: "Nationalities",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_NameEnglish",
                table: "Nationalities",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeOwners_Nationalities_NationalityId",
                table: "OfficeOwners",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeOwners_Nationalities_NationalityId",
                table: "OfficeOwners");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_OfficeOwners_NationalityId",
                table: "OfficeOwners");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "OfficeOwners");

            migrationBuilder.DropColumn(
                name: "NationalityId",
                table: "OfficeOwners");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "OfficeOwners");
        }
    }
}
