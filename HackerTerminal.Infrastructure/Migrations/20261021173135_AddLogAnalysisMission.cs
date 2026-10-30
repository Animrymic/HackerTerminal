using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLogAnalysisMission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Category", "ChallengePrompt", "CreatedAt", "Description", "Difficulty", "ExpectedAnswer", "IsActive", "TimeLimitSeconds", "Title", "XPReward" },
                values: new object[] { 3, "Log Analysis", "A fresh set of login events is generated when you start this mission.", new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Analyze login events and identify the source of repeated failed attempts.", "Medium", "10.0.0.1", true, 180, "Log Analysis", 200 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
