using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMastery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Summoner/{id}/ChampionMastery")]
public class SummonerChampionMasteryController : ControllerBase
{
    private readonly IMediator _mediator;

    public SummonerChampionMasteryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public Task<IActionResult> GetAll(Guid id) =>
        _mediator
            .Send(new GetSummonerChampionMasteryQuery(id))
            .ToIActionResult(this);
}