using System.Security.Claims;
using HackerTerminal.API.DTOs;
using HackerTerminal.Core.Entities;
using HackerTerminal.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HackerTerminal.API.Services;

namespace HackerTerminal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GameSessionsController : ControllerBase
{
    private readonly HackerTerminalDbContext _context;

    public GameSessionsController(HackerTerminalDbContext context)
    {
        _context = context;
    }

    [HttpPost("{scenarioId:int}/start")]
    public async Task<IActionResult> StartSession(int scenarioId)
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Unauthorized();
        }

        Scenario? scenario = await _context.Scenarios
            .SingleOrDefaultAsync(item => item.Id == scenarioId && item.IsActive);

        if (scenario is null)
        {
            return NotFound("Mission not found or inactive.");
        }

        if (!MissionChallengeGenerator.TryGenerate(
        scenario.Category,
        scenario.Difficulty,
        out string challengePrompt,
        out string expectedAnswer))
        {
            return BadRequest(
                "No challenge generator is configured for this mission category.");
        }

        GameSession session = new GameSession
        {
            UserId = userId,
            ScenarioId = scenario.Id,
            ChallengePrompt = challengePrompt,
            ExpectedAnswer = expectedAnswer,
            Score = 0,
            XPReward = 0,
            TimeTakenSeconds = 0,
            IsCompleted = false,
            StartedAt = DateTime.UtcNow
        };

        _context.GameSessions.Add(session);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            sessionId = session.Id,
            scenarioId = scenario.Id,
            challengePrompt = session.ChallengePrompt,
            scenarioTitle = scenario.Title,
            startedAt = session.StartedAt,
            timeLimitSeconds = scenario.TimeLimitSeconds
        });
    }

    [HttpPost("{sessionId:int}/submit")]
    public async Task<IActionResult> SubmitAnswer(
        int sessionId,
        [FromBody] SubmitAnswerDto request)
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Unauthorized();
        }

        GameSession? session = await _context.GameSessions
            .Include(item => item.Scenario)
            .SingleOrDefaultAsync(item =>
                item.Id == sessionId &&
                item.UserId == userId);

        if (session is null)
        {
            return NotFound("Game session not found.");
        }

        if (session.IsCompleted)
        {
            return BadRequest("This mission session is already complete.");
        }

        DateTime finishedAt = DateTime.UtcNow;
        int timeTakenSeconds = Math.Max(
            0,
            (int)(finishedAt - session.StartedAt).TotalSeconds);

        if (timeTakenSeconds >= session.Scenario.TimeLimitSeconds)
        {
            session.IsCompleted = true;
            session.TimeTakenSeconds = timeTakenSeconds;
            session.FinishedAt = finishedAt;
            session.Score = 0;
            session.XPReward = 0;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                isCorrect = false,
                isCompleted = true,
                message = "Time limit expired.",
                score = 0,
                xpReward = 0
            });
        }

        string expectedAnswer =
    session.ExpectedAnswer ?? session.Scenario.ExpectedAnswer;

        bool isCaseInsensitive =
    string.Equals(
        session.Scenario.Category,
        "Cipher",
        StringComparison.OrdinalIgnoreCase) ||
    string.Equals(
        session.Scenario.Category,
        "Base64",
        StringComparison.OrdinalIgnoreCase);

        StringComparison comparison = isCaseInsensitive
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        bool isCorrect = string.Equals(
            request.Answer.Trim(),
            expectedAnswer.Trim(),
            comparison);

        if (!isCorrect)
        {
            return Ok(new
            {
                isCorrect = false,
                isCompleted = false,
                message = "Incorrect answer. Try again."
            });
        }

        session.IsCompleted = true;
        session.TimeTakenSeconds = timeTakenSeconds;
        session.FinishedAt = finishedAt;
        session.Score = Math.Max(1,session.Scenario.TimeLimitSeconds - timeTakenSeconds);
        session.XPReward = session.Scenario.XPReward;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            isCorrect = true,
            isCompleted = true,
            message = "Correct answer!",
            score = session.Score,
            xpReward = session.XPReward,
            timeTakenSeconds = session.TimeTakenSeconds
        });
    }

    [HttpGet("my-stats")]
    public async Task<ActionResult<PlayerStatsDto>> GetMyStats()
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Unauthorized();
        }

        var completedSessions = await _context.GameSessions
            .Where(session =>
                session.UserId == userId &&
                session.IsCompleted &&
                session.Score > 0)
            .Select(session => new
            {
                session.Score,
                session.XPReward
            })
            .ToListAsync();

        PlayerStatsDto stats = new PlayerStatsDto
        {
            CompletedMissions = completedSessions.Count,
            TotalXp = completedSessions.Sum(session => session.XPReward),
            TotalScore = completedSessions.Sum(session => session.Score),
            BestScore = completedSessions.Count == 0
                ? 0
                : completedSessions.Max(session => session.Score)
        };

        return Ok(stats);
    }

    [HttpGet("leaderboard")]
    public async Task<ActionResult<List<LeaderboardEntryDto>>> GetLeaderboard(
    [FromQuery] string period = "all")
    {
        if (!TryGetLeaderboardCutoff(period, out DateTime? cutoff))
        {
            return BadRequest("Period must be 'all', 'week', or 'month'.");
        }

        IQueryable<GameSession> sessions = _context.GameSessions
            .Where(session =>
                session.IsCompleted &&
                session.Score > 0 &&
                session.User.IsActive);

        if (cutoff.HasValue)
        {
            sessions = sessions.Where(session => session.FinishedAt >= cutoff.Value);
        }

        var results = await sessions
            .GroupBy(session => new
            {
                session.UserId,
                session.User.Username
            })
            .Select(group => new
            {
                Username = group.Key.Username,
                CompletedMissions = group.Count(),
                TotalXp = group.Sum(session => session.XPReward),
                TotalScore = group.Sum(session => session.Score)
            })
            .OrderByDescending(entry => entry.TotalScore)
            .ThenByDescending(entry => entry.TotalXp)
            .ThenBy(entry => entry.Username)
            .Take(20)
            .ToListAsync();

        List<LeaderboardEntryDto> leaderboard = results
            .Select((entry, index) => new LeaderboardEntryDto
            {
                Rank = index + 1,
                Username = entry.Username,
                CompletedMissions = entry.CompletedMissions,
                TotalXp = entry.TotalXp,
                TotalScore = entry.TotalScore
            })
            .ToList();

        return Ok(leaderboard);
    }

    [HttpGet("my-history")]
    public async Task<ActionResult<List<GameSessionHistoryDto>>> GetMyHistory(
    [FromQuery] int page = 1)
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Unauthorized();
        }

        const int pageSize = 50;
        page = Math.Max(1, page);

        var history = await _context.GameSessions
            .Where(session => session.UserId == userId && session.IsCompleted)
            .OrderByDescending(session => session.FinishedAt)
            .ThenByDescending(session => session.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(session => new GameSessionHistoryDto
            {
                ScenarioTitle = session.Scenario.Title,
                Score = session.Score,
                XPReward = session.XPReward,
                TimeTakenSeconds = session.TimeTakenSeconds,
                IsSuccessful = session.Score > 0,
                FinishedAt = session.FinishedAt
            })
            .ToListAsync();

        return Ok(history);
    }

    [HttpGet("my-rank")]
    public async Task<ActionResult<PlayerRankDto>> GetMyRank(
    [FromQuery] string period = "all")
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Unauthorized();
        }

        if (!TryGetLeaderboardCutoff(period, out DateTime? cutoff))
        {
            return BadRequest("Period must be 'all', 'week', or 'month'.");
        }

        IQueryable<GameSession> sessions = _context.GameSessions
            .Where(session =>
                session.IsCompleted &&
                session.Score > 0 &&
                session.User.IsActive);

        if (cutoff.HasValue)
        {
            sessions = sessions.Where(session => session.FinishedAt >= cutoff.Value);
        }

        var standings = await sessions
            .GroupBy(session => new
            {
                session.UserId,
                session.User.Username
            })
            .Select(group => new
            {
                UserId = group.Key.UserId,
                Username = group.Key.Username,
                TotalXp = group.Sum(session => session.XPReward),
                TotalScore = group.Sum(session => session.Score),
                CompletedMissions = group.Count()
            })
            .OrderByDescending(player => player.TotalScore)
            .ThenByDescending(player => player.TotalXp)
            .ThenBy(player => player.Username)
            .ToListAsync();

        int playerIndex = standings.FindIndex(player => player.UserId == userId);

        PlayerRankDto result = playerIndex < 0
            ? new PlayerRankDto()
            : new PlayerRankDto
            {
                Rank = playerIndex + 1,
                TotalScore = standings[playerIndex].TotalScore,
                TotalXp = standings[playerIndex].TotalXp,
                CompletedMissions = standings[playerIndex].CompletedMissions
            };

        return Ok(result);
    }

    private static bool TryGetLeaderboardCutoff(
    string period,
    out DateTime? cutoff)
    {
        switch (period.Trim().ToLowerInvariant())
        {
            case "all":
                cutoff = null;
                return true;

            case "week":
                cutoff = DateTime.UtcNow.AddDays(-7);
                return true;

            case "month":
                cutoff = DateTime.UtcNow.AddDays(-30);
                return true;

            default:
                cutoff = null;
                return false;
        }
    }
}