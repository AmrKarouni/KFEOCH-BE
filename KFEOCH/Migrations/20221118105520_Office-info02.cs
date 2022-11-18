using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class Officeinfo02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "LegalEntityId",
                table: "Offices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EntityId",
                table: "Offices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AreaId",
                table: "Offices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Areas_AreaId",
                table: "Offices",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_OfficeEntities_EntityId",
                table: "Offices",
                column: "EntityId",
                principalTable: "OfficeEntities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_OfficeLegalEntities_LegalEntityId",
                table: "Offices",
                column: "LegalEntityId",
                principalTable: "OfficeLegalEntities",
                principalColumn: "Id");
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

            migrationBuilder.AlterColumn<int>(
                name: "LegalEntityId",
                table: "Offices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EntityId",
                table: "Offices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AreaId",
                table: "Offices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
