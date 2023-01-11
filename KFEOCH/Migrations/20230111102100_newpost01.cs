using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFEOCH.Migrations
{
    public partial class newpost01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostTypes_TypeId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "OfficeLicenses");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Posts",
                newName: "PageId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_TypeId",
                table: "Posts",
                newName: "IX_Posts_PageId");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEnglish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubTitleArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitleEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_PostTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "PostTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostId",
                table: "Posts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_TitleArabic",
                table: "Pages",
                column: "TitleArabic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_TitleEnglish",
                table: "Pages",
                column: "TitleEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_TypeId",
                table: "Pages",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Pages_PageId",
                table: "Posts",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_PostId",
                table: "Posts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Pages_PageId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_PostId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PostId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "PageId",
                table: "Posts",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_PageId",
                table: "Posts",
                newName: "IX_Posts_TypeId");

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BodyArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadlineArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadlineEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NewsDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShowInHome = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeLicenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true),
                    IsRejected = table.Column<bool>(type: "bit", nullable: true),
                    PaymentAmount = table.Column<double>(type: "float", nullable: true),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationStartDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeLicenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeLicenses_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeLicenses_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    BodyArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    TitleArabic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEnglish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    thumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_OfficeLicenses_OfficeId",
                table: "OfficeLicenses",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLicenses_PaymentTypeId",
                table: "OfficeLicenses",
                column: "PaymentTypeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostTypes_TypeId",
                table: "Posts",
                column: "TypeId",
                principalTable: "PostTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
