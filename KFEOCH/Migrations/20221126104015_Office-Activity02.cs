using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class OfficeActivity02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_officeActivities_Activities_ActivityId",
                table: "officeActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_officeActivities_Offices_OfficeId",
                table: "officeActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_officeActivities",
                table: "officeActivities");

            migrationBuilder.RenameTable(
                name: "officeActivities",
                newName: "OfficeActivities");

            migrationBuilder.RenameIndex(
                name: "IX_officeActivities_OfficeId_ActivityId",
                table: "OfficeActivities",
                newName: "IX_OfficeActivities_OfficeId_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_officeActivities_ActivityId",
                table: "OfficeActivities",
                newName: "IX_OfficeActivities_ActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeActivities",
                table: "OfficeActivities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeActivities_Activities_ActivityId",
                table: "OfficeActivities",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeActivities_Offices_OfficeId",
                table: "OfficeActivities",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeActivities_Activities_ActivityId",
                table: "OfficeActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeActivities_Offices_OfficeId",
                table: "OfficeActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeActivities",
                table: "OfficeActivities");

            migrationBuilder.RenameTable(
                name: "OfficeActivities",
                newName: "officeActivities");

            migrationBuilder.RenameIndex(
                name: "IX_OfficeActivities_OfficeId_ActivityId",
                table: "officeActivities",
                newName: "IX_officeActivities_OfficeId_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_OfficeActivities_ActivityId",
                table: "officeActivities",
                newName: "IX_officeActivities_ActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_officeActivities",
                table: "officeActivities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_officeActivities_Activities_ActivityId",
                table: "officeActivities",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_officeActivities_Offices_OfficeId",
                table: "officeActivities",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");
        }
    }
}
