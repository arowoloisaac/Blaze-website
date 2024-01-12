using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace startup_trial.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_restaurantRatings_Restaurants_RestaurantOwnerId",
                table: "restaurantRatings");

            migrationBuilder.DropIndex(
                name: "IX_restaurantRatings_RestaurantOwnerId",
                table: "restaurantRatings");

            migrationBuilder.DropColumn(
                name: "RestaurantOwnerId",
                table: "restaurantRatings");

            migrationBuilder.CreateIndex(
                name: "IX_restaurantRatings_RestaurantId",
                table: "restaurantRatings",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_restaurantRatings_Restaurants_RestaurantId",
                table: "restaurantRatings",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_restaurantRatings_Restaurants_RestaurantId",
                table: "restaurantRatings");

            migrationBuilder.DropIndex(
                name: "IX_restaurantRatings_RestaurantId",
                table: "restaurantRatings");

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantOwnerId",
                table: "restaurantRatings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_restaurantRatings_RestaurantOwnerId",
                table: "restaurantRatings",
                column: "RestaurantOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_restaurantRatings_Restaurants_RestaurantOwnerId",
                table: "restaurantRatings",
                column: "RestaurantOwnerId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
