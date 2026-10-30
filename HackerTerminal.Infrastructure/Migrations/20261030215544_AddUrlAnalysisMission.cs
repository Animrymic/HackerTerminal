using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUrlAnalysisMission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Category", "ChallengePrompt", "CreatedAt", "Description", "Difficulty", "ExpectedAnswer", "IsActive", "TimeLimitSeconds", "Title", "XPReward" },
                values: new object[] { 5, "URL Analysis", "A fresh URL analysis challenge is generated when you start this mission.", new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inspect suspicious links and identify the URL hosted by the trusted domain.", "Medium", "https://example.com/start", true, 150, "URL Analysis", 180 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
