using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace startup_trial.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantRatingToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantRating_AspNetUsers_UserId",
                table: "RestaurantRating");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantRating_Restaurants_RestaurantOwnerId",
                table: "RestaurantRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RestaurantRating",
                table: "RestaurantRating");

            migrationBuilder.RenameTable(
                name: "RestaurantRating",
                newName: "restaurantRatings");

            migrationBuilder.RenameIndex(
                name: "IX_RestaurantRating_UserId",
                table: "restaurantRatings",
                newName: "IX_restaurantRatings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RestaurantRating_RestaurantOwnerId",
                table: "restaurantRatings",
                newName: "IX_restaurantRatings_RestaurantOwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_restaurantRatings",
                table: "restaurantRatings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_restaurantRatings_AspNetUsers_UserId",
                table: "restaurantRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_restaurantRatings_Restaurants_RestaurantOwnerId",
                table: "restaurantRatings",
                column: "RestaurantOwnerId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_restaurantRatings_AspNetUsers_UserId",
                table: "restaurantRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_restaurantRatings_Restaurants_RestaurantOwnerId",
                table: "restaurantRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_restaurantRatings",
                table: "restaurantRatings");

            migrationBuilder.RenameTable(
                name: "restaurantRatings",
                newName: "RestaurantRating");

            migrationBuilder.RenameIndex(
                name: "IX_restaurantRatings_UserId",
                table: "RestaurantRating",
                newName: "IX_RestaurantRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_restaurantRatings_RestaurantOwnerId",
                table: "RestaurantRating",
                newName: "IX_RestaurantRating_RestaurantOwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestaurantRating",
                table: "RestaurantRating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantRating_AspNetUsers_UserId",
                table: "RestaurantRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantRating_Restaurants_RestaurantOwnerId",
                table: "RestaurantRating",
                column: "RestaurantOwnerId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
