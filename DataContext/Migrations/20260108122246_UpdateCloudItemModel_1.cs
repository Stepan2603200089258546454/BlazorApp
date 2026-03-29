using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCloudItemModel_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Parrent",
                table: "CloudItems",
                newName: "ParrentId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudItems_ParrentId",
                table: "CloudItems",
                column: "ParrentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItems_CloudItems_ParrentId",
                table: "CloudItems",
                column: "ParrentId",
                principalTable: "CloudItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudItems_CloudItems_ParrentId",
                table: "CloudItems");

            migrationBuilder.DropIndex(
                name: "IX_CloudItems_ParrentId",
                table: "CloudItems");

            migrationBuilder.RenameColumn(
                name: "ParrentId",
                table: "CloudItems",
                newName: "Parrent");
        }
    }
}
