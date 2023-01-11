using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class newpost02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "thumbnailUrl",
                table: "Posts",
                newName: "ThumbnailUrl");

            migrationBuilder.RenameColumn(
                name: "thumbnailUrl",
                table: "Pages",
                newName: "ThumbnailUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "Posts",
                newName: "thumbnailUrl");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "Pages",
                newName: "thumbnailUrl");
        }
    }
}
