using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSqlInjectionMissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Category", "ChallengePrompt", "CreatedAt", "Description", "Difficulty", "ExpectedAnswer", "IsActive", "TimeLimitSeconds", "Title", "XPReward" },
                values: new object[,]
                {
                    { 14, "SQL Injection", "A fresh SQL injection challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Recognize a basic SQL injection pattern in a training-only login query.", "Easy", "DYNAMIC", true, 150, "SQL Injection Basics", 175 },
                    { 15, "SQL Injection", "A fresh SQL injection challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Analyze input choices and identify the SQL injection pattern.", "Medium", "DYNAMIC", true, 180, "SQL Injection Analysis // MEDIUM", 225 },
                    { 16, "SQL Injection", "A fresh hard SQL injection challenge is generated when you start this mission.", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Identify an SQL injection pattern from a larger set of possible inputs.", "Hard", "DYNAMIC", true, 210, "SQL Injection Analysis // HARD", 275 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
