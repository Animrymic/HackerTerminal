using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPhishingMissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Category", "ChallengePrompt", "CreatedAt", "Description", "Difficulty", "ExpectedAnswer", "IsActive", "TimeLimitSeconds", "Title", "XPReward" },
                values: new object[,]
                {
                    { 11, "Phishing", "A fresh phishing challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inspect a suspicious email sender and identify the deceptive address.", "Easy", "DYNAMIC", true, 150, "Phishing Detection", 150 },
                    { 12, "Phishing", "A fresh phishing challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Analyze email sender addresses for deceptive domain patterns.", "Medium", "DYNAMIC", true, 180, "Phishing Detection // MEDIUM", 200 },
                    { 13, "Phishing", "A fresh hard phishing challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Analyze a larger set of sender addresses for deceptive domain patterns.", "Hard", "DYNAMIC", true, 210, "Phishing Detection // HARD", 250 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 13);
        }
    }
}
