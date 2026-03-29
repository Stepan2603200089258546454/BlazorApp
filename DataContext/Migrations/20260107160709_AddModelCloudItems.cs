using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddModelCloudItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CloudFileData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Md5Hash = table.Column<string>(type: "text", nullable: false),
                    Sha256Hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudFileData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalClouds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemName = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalClouds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalClouds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CloudItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    SystemName = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Parrent = table.Column<Guid>(type: "uuid", nullable: true),
                    PersonalCloudId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileDataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CloudItem_CloudFileData_FileDataId",
                        column: x => x.FileDataId,
                        principalTable: "CloudFileData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CloudItem_PersonalClouds_PersonalCloudId",
                        column: x => x.PersonalCloudId,
                        principalTable: "PersonalClouds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudItem_FileDataId",
                table: "CloudItem",
                column: "FileDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudItem_PersonalCloudId",
                table: "CloudItem",
                column: "PersonalCloudId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalClouds_UserId",
                table: "PersonalClouds",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloudItem");

            migrationBuilder.DropTable(
                name: "CloudFileData");

            migrationBuilder.DropTable(
                name: "PersonalClouds");
        }
    }
}
