using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace startup_trial.Migrations
{
    /// <inheritdoc />
    public partial class updateDriverObjectToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_DriverId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_DriverId",
                table: "Order");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DriverId",
                table: "Order",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_DriverId",
                table: "Order",
                column: "DriverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
