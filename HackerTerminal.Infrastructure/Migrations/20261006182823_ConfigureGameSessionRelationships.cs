using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HackerTerminal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureGameSessionRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_ScenarioId",
                table: "GameSessions",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_UserId",
                table: "GameSessions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Scenarios_ScenarioId",
                table: "GameSessions",
                column: "ScenarioId",
                principalTable: "Scenarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Users_UserId",
                table: "GameSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Scenarios_ScenarioId",
                table: "GameSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Users_UserId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_ScenarioId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_UserId",
                table: "GameSessions");
        }
    }
}
