using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.SummonerChampionMastery.Queries.GetSummonersByName;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SummonerController : ControllerBase
{
    private readonly IMediator _mediator;

    public SummonerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetByGameNameAndTagLine(string gameName, string tagLine, string region) => 
        (await _mediator.Send(new GetSummonerRequest(gameName, tagLine, region)))
        .ToIActionResult(this);
}