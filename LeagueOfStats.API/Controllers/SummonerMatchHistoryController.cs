using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchById;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;
using LeagueOfStats.Domain.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Summoner/{summonerId:guid}/MatchHistory")]
public class SummonerMatchHistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public SummonerMatchHistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public Task<IActionResult> GetMatchHistorySummary(Guid summonerId, [FromQuery] Instant gameEndedAt, [FromQuery] GameType gameType, [FromQuery] int limit = 20) =>
        _mediator.Send(new GetSummonerMatchHistorySummaryQuery(summonerId, gameEndedAt, gameType, limit))
            .ToIActionResult(this);
    
    [HttpGet("{id:guid}")]
    public Task<IActionResult> Get(Guid summonerId, Guid id) =>
        _mediator.Send(new GetSummonerMatchByIdQuery(summonerId, id))
            .ToIActionResult(this);
}