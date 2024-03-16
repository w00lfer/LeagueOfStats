using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Summoners/{summonerId:guid}/Challenges")]
[ApiExplorerSettings(IgnoreApi = true)]
public class SummonerChallengesController : ControllerBase
{
    [HttpGet()]
    public Task<IActionResult> GetSummonerChallenges(Guid summonerId) =>
        throw new NotImplementedException();
}