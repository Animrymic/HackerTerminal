using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHardMissionVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Category", "ChallengePrompt", "CreatedAt", "Description", "Difficulty", "ExpectedAnswer", "IsActive", "TimeLimitSeconds", "Title", "XPReward" },
                values: new object[,]
                {
                    { 6, "Password", "A new hard password challenge is generated when you start this mission.", new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Evaluate a complex password challenge under a tighter time limit.", "Hard", "M7!vQ2#pL9", true, 150, "Password Breach // HARD", 160 },
                    { 7, "Cipher", "A new hard Caesar cipher is generated when you start this mission.", new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Decode a longer intercepted message before time runs out.", "Hard", "TERMINALCODE", true, 240, "Cipher Break // HARD", 220 },
                    { 8, "Log Analysis", "A new hard set of login events is generated when you start this mission.", new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Analyze a larger set of events to find the source of repeated failed logins.", "Hard", "10.0.0.1", true, 240, "Log Analysis // HARD", 250 },
                    { 9, "Base64", "A new hard Base64 message is generated when you start this mission.", new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Decode a longer intercepted Base64 message.", "Hard", "TERMINAL1234", true, 240, "Base64 Decode // HARD", 220 },
                    { 10, "URL Analysis", "A new hard URL analysis challenge is generated when you start this mission.", new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inspect more deceptive URLs and identify the exact trusted hostname.", "Hard", "https://example.com/start", true, 210, "URL Analysis // HARD", 240 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
