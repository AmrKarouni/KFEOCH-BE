using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class OwnerDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OwnerDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnerDocuments_OfficeOwners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "OfficeOwners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OwnerDocuments_OwnerDocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "OwnerDocumentTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDocuments_OwnerId",
                table: "OwnerDocuments",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDocuments_TypeId",
                table: "OwnerDocuments",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OwnerDocuments");
        }
    }
}
