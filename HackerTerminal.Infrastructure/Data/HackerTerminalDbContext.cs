using HackerTerminal.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HackerTerminal.Infrastructure.Data;

public class HackerTerminalDbContext : DbContext
{
    public HackerTerminalDbContext(DbContextOptions<HackerTerminalDbContext> options) 
        : base(options)

    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Scenario> Scenarios { get; set; }
    public DbSet<GameSession> GameSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
              .HasIndex(user => user.Username)
              .IsUnique();

        modelBuilder.Entity<User>()
              .HasIndex(user => user.Email)
              .IsUnique();

        modelBuilder.Entity<GameSession>()
              .HasOne(gameSession => gameSession.User)
              .WithMany(user => user.GameSessions)
              .HasForeignKey(gameSession => gameSession.UserId)
              .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GameSession>()
              .HasOne(gameSession => gameSession.Scenario)
              .WithMany(scenario => scenario.GameSessions)
              .HasForeignKey(gameSession => gameSession.ScenarioId)
              .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Scenario>().HasData(
        new Scenario
        {
            Id = 1,
            Title = "Password Breach",
            Description = "A suspicious account has been compromised. Identify the correct password before the attacker gains access.",
            Category = "Password",
            Difficulty = "Easy",
            ChallengePrompt = "A new password challenge is generated when you start this mission.",
            ExpectedAnswer = "M7!vQ2#pL9",
            XPReward = 100,
            TimeLimitSeconds = 120,
            IsActive = true,
            CreatedAt = new DateTime(2026, 10, 7)
        },
        new Scenario
        {
            Id = 2,
            Title = "Cipher Break",
            Description = "Decode an intercepted message before time runs out.",
            Category = "Cipher",
            Difficulty = "Medium",
            ChallengePrompt = "A new Caesar cipher is generated when you start this mission.",
            ExpectedAnswer = "TERMINAL",
            XPReward = 150,
            TimeLimitSeconds = 180,
            IsActive = true,
            CreatedAt = new DateTime(2026, 10, 21)
        },
        new Scenario
        {
            Id = 3,
            Title = "Log Analysis",
            Description = "Analyze login events and identify the source of repeated failed attempts.",
            Category = "Log Analysis",
            Difficulty = "Medium",
            ChallengePrompt = "A fresh set of login events is generated when you start this mission.",
            ExpectedAnswer = "10.0.0.1",
            XPReward = 200,
            TimeLimitSeconds = 180,
            IsActive = true,
            CreatedAt = new DateTime(2026, 10, 21)
        },
        new Scenario
        {
            Id = 4,
            Title = "Base64 Decode",
            Description = "Decode an intercepted Base64 message.",
            Category = "Base64",
            Difficulty = "Medium",
            ChallengePrompt = "A fresh Base64 message is generated when you start this mission.",
            ExpectedAnswer = "TERMINAL",
            XPReward = 175,
            TimeLimitSeconds = 180,
            IsActive = true,
            CreatedAt = new DateTime(2026, 10, 21)
        },

        new Scenario
        {
            Id = 5,
            Title = "URL Analysis",
            Description = "Inspect suspicious links and identify the URL hosted by the trusted domain.",
            Category = "URL Analysis",
            Difficulty = "Medium",
            ChallengePrompt = "A fresh URL analysis challenge is generated when you start this mission.",
            ExpectedAnswer = "https://example.com/start",
            XPReward = 180,
            TimeLimitSeconds = 150,
            IsActive = true,
            CreatedAt = new DateTime(2026, 10, 30)
        }
        );
    }
}
