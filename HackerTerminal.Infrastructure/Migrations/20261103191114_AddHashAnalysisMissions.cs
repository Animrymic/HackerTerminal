using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHashAnalysisMissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Category", "ChallengePrompt", "CreatedAt", "Description", "Difficulty", "ExpectedAnswer", "IsActive", "TimeLimitSeconds", "Title", "XPReward" },
                values: new object[,]
                {
                    { 20, "Hash Analysis", "A fresh hash analysis challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Identify the SHA-256 digest for a short message.", "Easy", "DYNAMIC", true, 180, "Hash Analysis", 200 },
                    { 21, "Hash Analysis", "A fresh hash analysis challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Identify the SHA-256 digest for a longer message among more choices.", "Medium", "DYNAMIC", true, 210, "Hash Analysis // MEDIUM", 250 },
                    { 22, "Hash Analysis", "A fresh hard hash analysis challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Identify the SHA-256 digest for a longer message from the largest choice set.", "Hard", "DYNAMIC", true, 240, "Hash Analysis // HARD", 300 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 22);
        }
    }
}
