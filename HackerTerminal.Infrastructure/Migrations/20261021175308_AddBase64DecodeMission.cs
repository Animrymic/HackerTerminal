using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBase64DecodeMission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Category", "ChallengePrompt", "CreatedAt", "Description", "Difficulty", "ExpectedAnswer", "IsActive", "TimeLimitSeconds", "Title", "XPReward" },
                values: new object[] { 4, "Base64", "A fresh Base64 message is generated when you start this mission.", new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Decode an intercepted Base64 message.", "Medium", "TERMINAL", true, 180, "Base64 Decode", 175 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
