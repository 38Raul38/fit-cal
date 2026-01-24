using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Carbs",
                table: "UserInformations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DailyCalories",
                table: "UserInformations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fats",
                table: "UserInformations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Protein",
                table: "UserInformations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carbs",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "DailyCalories",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "Fats",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "UserInformations");
        }
    }
}
