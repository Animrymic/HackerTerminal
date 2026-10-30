using System;
using System.Collections.Generic;
using System.Text;

namespace HackerTerminal.API.DTOs;

public class PlayerStatsDto
{
    public int CompletedMissions { get; set; }
    public int TotalXp { get; set; }
    public int TotalScore { get; set; }
    public int BestScore { get; set; }
}
