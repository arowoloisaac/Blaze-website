using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace startup_trial.Migrations
{
    /// <inheritdoc />
    public partial class RemovePropertisFromDriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Accepted",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "AspNetUsers",
                type: "float",
                nullable: true);
        }
    }
}
