namespace HackerTerminal.Core.Entities;

public class GameSession
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ScenarioId { get; set; }
    public string? ChallengePrompt { get; set; }
    public string? ExpectedAnswer { get; set; }
    public int Score { get; set; }
    public int XPReward { get; set; }
    public int IncorrectAttempts { get; set; }
    public int TimeTakenSeconds { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public User User { get; set; } = null!;
    public Scenario Scenario { get; set; } = null!;
    public bool AttemptsTracked { get; set; }
}