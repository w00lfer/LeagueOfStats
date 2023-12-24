using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMasteries;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMastery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Summoner/{summonerId}/ChampionMastery")]
public class SummonerChampionMasteryController : ControllerBase
{
    private readonly IMediator _mediator;

    public SummonerChampionMasteryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public Task<IActionResult> GetAll(Guid summonerId) =>
        _mediator
            .Send(new GetSummonerChampionMasteriesQuery(summonerId))
            .ToIActionResult(this);
}