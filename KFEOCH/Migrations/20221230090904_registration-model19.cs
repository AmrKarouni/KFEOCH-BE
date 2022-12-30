using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class registrationmodel19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLast",
                table: "Licenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "Licenses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLast",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "Licenses");
        }
    }
}
