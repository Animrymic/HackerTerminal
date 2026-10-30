using HackerTerminal.API.DTOs;
using HackerTerminal.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackerTerminal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScenariosController : ControllerBase
{
    private readonly HackerTerminalDbContext _context;

    public ScenariosController(HackerTerminalDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<ScenarioListDto>>> GetScenarios()
    {
        List<ScenarioListDto> scenarios = await _context.Scenarios
            .Where(scenario => scenario.IsActive)
            .OrderBy(scenario => scenario.Id)
            .Select(scenario => new ScenarioListDto
            {
                Id = scenario.Id,
                Title = scenario.Title,
                Description = scenario.Description,
                Category = scenario.Category,
                Difficulty = scenario.Difficulty,
                XPReward = scenario.XPReward,
                ChallengePrompt = scenario.ChallengePrompt,
                TimeLimitSeconds = scenario.TimeLimitSeconds,
                IsActive = scenario.IsActive,
                CreatedAt = scenario.CreatedAt
            })
            .ToListAsync();

        return Ok(scenarios);
    }
}