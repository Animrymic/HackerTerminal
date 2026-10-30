namespace HackerTerminal.API.DTOs;

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public string Username { get; set; } = string.Empty;
    public int CompletedMissions { get; set; }
    public int TotalXp { get; set; }
    public int TotalScore { get; set; }
}