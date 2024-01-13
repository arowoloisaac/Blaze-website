using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace startup_trial.Migrations
{
    /// <inheritdoc />
    public partial class ImplementedTheRestaurantDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Restaurants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Restaurants",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Order_RestaurantId",
                table: "Order",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Restaurants_RestaurantId",
                table: "Order",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Restaurants_RestaurantId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_RestaurantId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Order");
        }
    }
}
