using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Summoners.Enums;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchById;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Summoners/{summonerId:guid}/MatchHistory")]
public class SummonerMatchHistoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IClock _clock;

    public SummonerMatchHistoryController(
        IMediator mediator,
        IClock clock)
    {
        _mediator = mediator;
        _clock = clock;
    }
    
    [HttpGet]
    public Task<IActionResult> GetMatchHistorySummary(
        Guid summonerId,
        [FromQuery] MatchHistoryQueueFilter matchHistoryQueueFilter = MatchHistoryQueueFilter.All,
        [FromQuery] int limit = 5,
        [FromQuery] Instant? gameEndedAt = null) =>
        _mediator.Send(new GetSummonerMatchHistorySummaryQuery(
                summonerId,
                gameEndedAt ?? _clock.GetCurrentInstant(),
                matchHistoryQueueFilter,
                limit > 5 
                    ? 5 
                    : limit))
            .ToIActionResult(this);
    
    [HttpGet("{id:guid}")]
    public Task<IActionResult> Get(Guid summonerId, Guid id) =>
        _mediator.Send(new GetSummonerMatchByIdQuery(summonerId, id))
            .ToIActionResult(this);
}