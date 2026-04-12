using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    /// <inheritdoc />
    public partial class UploadCloudItemModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CloudItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CloudItems_UserId",
                table: "CloudItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItems_AspNetUsers_UserId",
                table: "CloudItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudItems_AspNetUsers_UserId",
                table: "CloudItems");

            migrationBuilder.DropIndex(
                name: "IX_CloudItems_UserId",
                table: "CloudItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CloudItems");
        }
    }
}
