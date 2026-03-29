using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudItem_CloudFileData_FileDataId",
                table: "CloudItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CloudItem_PersonalClouds_PersonalCloudId",
                table: "CloudItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CloudItem",
                table: "CloudItem");

            migrationBuilder.RenameTable(
                name: "CloudItem",
                newName: "CloudItems");

            migrationBuilder.RenameIndex(
                name: "IX_CloudItem_PersonalCloudId",
                table: "CloudItems",
                newName: "IX_CloudItems_PersonalCloudId");

            migrationBuilder.RenameIndex(
                name: "IX_CloudItem_FileDataId",
                table: "CloudItems",
                newName: "IX_CloudItems_FileDataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CloudItems",
                table: "CloudItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItems_CloudFileData_FileDataId",
                table: "CloudItems",
                column: "FileDataId",
                principalTable: "CloudFileData",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItems_PersonalClouds_PersonalCloudId",
                table: "CloudItems",
                column: "PersonalCloudId",
                principalTable: "PersonalClouds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudItems_CloudFileData_FileDataId",
                table: "CloudItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CloudItems_PersonalClouds_PersonalCloudId",
                table: "CloudItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CloudItems",
                table: "CloudItems");

            migrationBuilder.RenameTable(
                name: "CloudItems",
                newName: "CloudItem");

            migrationBuilder.RenameIndex(
                name: "IX_CloudItems_PersonalCloudId",
                table: "CloudItem",
                newName: "IX_CloudItem_PersonalCloudId");

            migrationBuilder.RenameIndex(
                name: "IX_CloudItems_FileDataId",
                table: "CloudItem",
                newName: "IX_CloudItem_FileDataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CloudItem",
                table: "CloudItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItem_CloudFileData_FileDataId",
                table: "CloudItem",
                column: "FileDataId",
                principalTable: "CloudFileData",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItem_PersonalClouds_PersonalCloudId",
                table: "CloudItem",
                column: "PersonalCloudId",
                principalTable: "PersonalClouds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
