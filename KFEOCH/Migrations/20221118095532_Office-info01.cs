using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class Officeinfo01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Offices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                table: "Offices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstablishmentDate",
                table: "Offices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LegalEntityId",
                table: "Offices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseEndDate",
                table: "Offices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_AreaId",
                table: "Offices",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_EntityId",
                table: "Offices",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_LegalEntityId",
                table: "Offices",
                column: "LegalEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Areas_AreaId",
                table: "Offices",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_OfficeEntities_EntityId",
                table: "Offices",
                column: "EntityId",
                principalTable: "OfficeEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_OfficeLegalEntities_LegalEntityId",
                table: "Offices",
                column: "LegalEntityId",
                principalTable: "OfficeLegalEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Areas_AreaId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_OfficeEntities_EntityId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_OfficeLegalEntities_LegalEntityId",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_AreaId",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_EntityId",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_LegalEntityId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "EstablishmentDate",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "LicenseEndDate",
                table: "Offices");
        }
    }
}
