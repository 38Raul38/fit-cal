using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerAuthUserId",
                table: "Foods",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Foods_OwnerAuthUserId",
                table: "Foods",
                column: "OwnerAuthUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Foods_OwnerAuthUserId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "OwnerAuthUserId",
                table: "Foods");
        }
    }
}
