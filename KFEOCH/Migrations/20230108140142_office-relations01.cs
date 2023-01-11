using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class officerelations01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OfficePayments_TypeId",
                table: "OfficePayments",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficePayments_PaymentTypes_TypeId",
                table: "OfficePayments",
                column: "TypeId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficePayments_PaymentTypes_TypeId",
                table: "OfficePayments");

            migrationBuilder.DropIndex(
                name: "IX_OfficePayments_TypeId",
                table: "OfficePayments");
        }
    }
}
