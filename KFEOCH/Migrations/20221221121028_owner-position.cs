using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class ownerposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "OfficeOwners",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OwnerPositionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerPositionTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfficeOwners_PositionId",
                table: "OfficeOwners",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerPositionTypes_NameArabic",
                table: "OwnerPositionTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OwnerPositionTypes_NameEnglish",
                table: "OwnerPositionTypes",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeOwners_OwnerPositionTypes_PositionId",
                table: "OfficeOwners",
                column: "PositionId",
                principalTable: "OwnerPositionTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeOwners_OwnerPositionTypes_PositionId",
                table: "OfficeOwners");

            migrationBuilder.DropTable(
                name: "OwnerPositionTypes");

            migrationBuilder.DropIndex(
                name: "IX_OfficeOwners_PositionId",
                table: "OfficeOwners");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "OfficeOwners");
        }
    }
}
