namespace HackerTerminal.API.DTOs;

public class GameSessionHistoryDto
{
    public string ScenarioTitle { get; set; } = string.Empty;
    public int Score { get; set; }
    public int XPReward { get; set; }
    public int IncorrectAttempts { get; set; }
    public int TimeTakenSeconds { get; set; }
    public bool IsSuccessful { get; set; }
    public DateTime? FinishedAt { get; set; }
}