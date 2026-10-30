namespace HackerTerminal.Core.Entities;

public class Scenario
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;

    public string ChallengePrompt { get; set; } = string.Empty;
    public string ExpectedAnswer { get; set; } = string.Empty;

    public int XPReward { get; set; }
    public int TimeLimitSeconds { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
}