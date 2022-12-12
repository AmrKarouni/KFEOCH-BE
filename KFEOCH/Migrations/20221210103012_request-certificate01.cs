using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class requestcertificate01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CertificateId",
                table: "RequestTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestTypes_CertificateId",
                table: "RequestTypes",
                column: "CertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestTypes_CertificateEntities_CertificateId",
                table: "RequestTypes",
                column: "CertificateId",
                principalTable: "CertificateEntities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestTypes_CertificateEntities_CertificateId",
                table: "RequestTypes");

            migrationBuilder.DropIndex(
                name: "IX_RequestTypes_CertificateId",
                table: "RequestTypes");

            migrationBuilder.DropColumn(
                name: "CertificateId",
                table: "RequestTypes");
        }
    }
}
