using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace startup_trial.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantIdToDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantId",
                table: "Dishes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Dishes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_UserId",
                table: "Dishes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_AspNetUsers_UserId",
                table: "Dishes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_AspNetUsers_UserId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_UserId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Dishes");
        }
    }
}
