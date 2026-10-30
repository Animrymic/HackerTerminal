using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialScenarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "Difficulty", "IsActive", "TimeLimitSeconds", "Title", "XPReward" },
                values: new object[] { 1, "Password", new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "A suspicious account has been compromised. Identify the correct password before the attacker gains access.", "Easy", true, 120, "Password Breach", 100 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
