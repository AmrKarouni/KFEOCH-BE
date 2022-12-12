using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class officedocument01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfficeDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeDocuments_OfficeDocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "OfficeDocumentTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfficeDocuments_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfficeDocuments_OfficeId",
                table: "OfficeDocuments",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeDocuments_TypeId",
                table: "OfficeDocuments",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficeDocuments");
        }
    }
}
