using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;
using LeagueOfStats.Domain.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Summoner/{summonerId}/MatchHistory")]
public class SummonerMatchHistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public SummonerMatchHistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public Task<IActionResult> GetAll(Guid summonerId, [FromQuery] Instant gameEndedAt, [FromQuery] GameType gameType, [FromQuery] int limit = 20) =>
        _mediator.Send(new GetSummonerMatchHistoryQuery(summonerId, gameEndedAt, gameType, limit))
            .ToIActionResult(this);
}