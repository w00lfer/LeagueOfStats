using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.SummonerChampionMastery.Queries.GetSummonerChampionMastery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SummonerChampionMasteryController : ControllerBase
{
    private readonly IMediator _mediator;

    public SummonerChampionMasteryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{puuid}")]
    public async Task<IActionResult> Get(string puuid, string region) =>
        (await _mediator.Send(new GetSummonerChampionMasteryRequest(puuid, region)))
        .ToIActionResult(this);
}