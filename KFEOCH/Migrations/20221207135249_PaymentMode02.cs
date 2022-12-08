using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class PaymentMode02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficePayments_PaymentTypes_TypeId",
                table: "OfficePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeRequests_OfficePayments_PaymentId",
                table: "OfficeRequests");

            migrationBuilder.DropIndex(
                name: "IX_OfficeRequests_PaymentId",
                table: "OfficeRequests");

            migrationBuilder.DropIndex(
                name: "IX_OfficePayments_TypeId",
                table: "OfficePayments");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "OfficeRequests");

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "OfficePayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OfficePayments_RequestId",
                table: "OfficePayments",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficePayments_OfficeRequests_RequestId",
                table: "OfficePayments",
                column: "RequestId",
                principalTable: "OfficeRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficePayments_OfficeRequests_RequestId",
                table: "OfficePayments");

            migrationBuilder.DropIndex(
                name: "IX_OfficePayments_RequestId",
                table: "OfficePayments");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "OfficePayments");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "OfficeRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeRequests_PaymentId",
                table: "OfficeRequests",
                column: "PaymentId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeRequests_OfficePayments_PaymentId",
                table: "OfficeRequests",
                column: "PaymentId",
                principalTable: "OfficePayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
