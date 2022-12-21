using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class Post01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEnglish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubTitleArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitleEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_PostTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "PostTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEnglish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BodyArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TitleArabic",
                table: "Posts",
                column: "TitleArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TitleEnglish",
                table: "Posts",
                column: "TitleEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TypeId",
                table: "Posts",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTypes_NameArabic",
                table: "PostTypes",
                column: "NameArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostTypes_NameEnglish",
                table: "PostTypes",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_PostId",
                table: "Sections",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_TitleArabic",
                table: "Sections",
                column: "TitleArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_TitleEnglish",
                table: "Sections",
                column: "TitleEnglish",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "PostTypes");
        }
    }
}
