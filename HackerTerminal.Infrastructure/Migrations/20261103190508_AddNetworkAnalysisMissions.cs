using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNetworkAnalysisMissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Category", "ChallengePrompt", "CreatedAt", "Description", "Difficulty", "ExpectedAnswer", "IsActive", "TimeLimitSeconds", "Title", "XPReward" },
                values: new object[,]
                {
                    { 17, "Network Analysis", "A fresh network port challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Identify the common network port associated with a service.", "Easy", "DYNAMIC", true, 150, "Network Port Analysis", 175 },
                    { 18, "Network Analysis", "A fresh network port challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Match network services to their commonly used ports.", "Medium", "DYNAMIC", true, 180, "Network Port Analysis // MEDIUM", 225 },
                    { 19, "Network Analysis", "A fresh hard network port challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Match network services to their commonly used ports from more choices.", "Hard", "DYNAMIC", true, 210, "Network Port Analysis // HARD", 275 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 19);
        }
    }
}
