using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCloudItemModel_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudItems_CloudFileData_FileDataId",
                table: "CloudItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItems_CloudFileData_FileDataId",
                table: "CloudItems",
                column: "FileDataId",
                principalTable: "CloudFileData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudItems_CloudFileData_FileDataId",
                table: "CloudItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItems_CloudFileData_FileDataId",
                table: "CloudItems",
                column: "FileDataId",
                principalTable: "CloudFileData",
                principalColumn: "Id");
        }
    }
}
