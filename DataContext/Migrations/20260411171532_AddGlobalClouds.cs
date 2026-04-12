using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddGlobalClouds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudItems_PersonalClouds_PersonalCloudId",
                table: "CloudItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "PersonalCloudId",
                table: "CloudItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "GlobalCloudId",
                table: "CloudItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GlobalClouds",
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
                    table.PrimaryKey("PK_GlobalClouds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlobalClouds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudItems_GlobalCloudId",
                table: "CloudItems",
                column: "GlobalCloudId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalClouds_UserId",
                table: "GlobalClouds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItems_GlobalClouds_GlobalCloudId",
                table: "CloudItems",
                column: "GlobalCloudId",
                principalTable: "GlobalClouds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItems_PersonalClouds_PersonalCloudId",
                table: "CloudItems",
                column: "PersonalCloudId",
                principalTable: "PersonalClouds",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudItems_GlobalClouds_GlobalCloudId",
                table: "CloudItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CloudItems_PersonalClouds_PersonalCloudId",
                table: "CloudItems");

            migrationBuilder.DropTable(
                name: "GlobalClouds");

            migrationBuilder.DropIndex(
                name: "IX_CloudItems_GlobalCloudId",
                table: "CloudItems");

            migrationBuilder.DropColumn(
                name: "GlobalCloudId",
                table: "CloudItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "PersonalCloudId",
                table: "CloudItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CloudItems_PersonalClouds_PersonalCloudId",
                table: "CloudItems",
                column: "PersonalCloudId",
                principalTable: "PersonalClouds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
