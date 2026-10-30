using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneratedChallengeToGameSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChallengePrompt",
                table: "GameSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedAnswer",
                table: "GameSessions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChallengePrompt",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "ExpectedAnswer",
                table: "GameSessions");
        }
    }
}
