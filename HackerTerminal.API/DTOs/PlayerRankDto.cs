namespace HackerTerminal.API.DTOs;

public class PlayerRankDto
{
    public int? Rank { get; set; }
    public int TotalScore { get; set; }
    public int TotalXp { get; set; }
    public int CompletedMissions { get; set; }
}
