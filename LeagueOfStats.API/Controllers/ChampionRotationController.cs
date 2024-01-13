using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("ChampionRotations")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ChampionRotationController : ControllerBase
{
    [HttpGet()]
    public Task<IActionResult> GetRecentChampionRotations(
        [FromQuery] Instant? rotationsFromDate = null) =>
        throw new NotImplementedException();

    [HttpGet("{id:guid}")]
    public Task<IActionResult> Get(Guid id) =>
        throw new NotImplementedException();
}