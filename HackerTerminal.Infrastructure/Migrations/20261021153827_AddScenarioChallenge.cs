using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddScenarioChallenge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChallengePrompt",
                table: "Scenarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExpectedAnswer",
                table: "Scenarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ChallengePrompt", "ExpectedAnswer" },
                values: new object[] { "Choose the strongest password: password123, qwerty2026, or M7!vQ2#pL9", "M7!vQ2#pL9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChallengePrompt",
                table: "Scenarios");

            migrationBuilder.DropColumn(
                name: "ExpectedAnswer",
                table: "Scenarios");
        }
    }
}
