using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMastery;
using LeagueOfStats.Domain.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Summoner/{id}/[controller]")]
public class SummonerChampionMasteryController : ControllerBase
{
    private readonly IMediator _mediator;

    public SummonerChampionMasteryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid id) =>
        (await _mediator.Send(new GetSummonerChampionMasteryRequest(id)))
        .ToIActionResult(this);
}