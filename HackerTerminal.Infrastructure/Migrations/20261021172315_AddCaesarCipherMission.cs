using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCaesarCipherMission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "ChallengePrompt",
                value: "A new password challenge is generated when you start this mission.");

            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Category", "ChallengePrompt", "CreatedAt", "Description", "Difficulty", "ExpectedAnswer", "IsActive", "TimeLimitSeconds", "Title", "XPReward" },
                values: new object[] { 2, "Cipher", "A new Caesar cipher is generated when you start this mission.", new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Decode an intercepted message before time runs out.", "Medium", "TERMINAL", true, 180, "Cipher Break", 150 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "ChallengePrompt",
                value: "Choose the strongest password: password123, qwerty2026, or M7!vQ2#pL9");
        }
    }
}
