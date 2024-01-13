using LeagueOfStats.Application.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Leagues")]
[ApiExplorerSettings(IgnoreApi = true)]
public class LeagueController : ControllerBase
{
    [HttpGet("Master")]
    public Task<IActionResult> GetMasterLeagueLeaderboard(LeagueQueueFilter leagueQueueFilter) =>
        throw new NotImplementedException();
    
    [HttpGet("GrandMaster")]
    public Task<IActionResult> GetGrandMasterLeagueLeaderboard(LeagueQueueFilter leagueQueueFilter) =>
        throw new NotImplementedException();
    
    [HttpGet("GrandMaster/Cutoff")]
    public Task<IActionResult> GetGrandMasterLeagueCutoff(LeagueQueueFilter leagueQueueFilter) =>
        throw new NotImplementedException();
    
    [HttpGet("Challenger")]
    public Task<IActionResult> GetChallengerLeagueLeaderboard(LeagueQueueFilter leagueQueueFilter) =>
        throw new NotImplementedException();
    
    [HttpGet("Challenger/Cutoff")]
    public Task<IActionResult> GetChallengerLeagueCutoff(LeagueQueueFilter leagueQueueFilter) =>
        throw new NotImplementedException();
}