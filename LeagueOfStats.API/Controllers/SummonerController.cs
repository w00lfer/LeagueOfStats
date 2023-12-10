using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerByGameNameAndTagLineAndRegion;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerById;
using LeagueOfStats.Domain.Common.Enums;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) =>
        (await _mediator.Send(new GetSummonerByIdRequest(id)))
        .ToIActionResult(this);
    
    [HttpGet]
    public async Task<IActionResult> GetByGameNameAndTagLine([FromQuery] string gameName, [FromQuery] string tagLine, [FromQuery] Region region) => 
        (await _mediator.Send(new GetSummonerByGameNameAndTagLineAndRegionRequest(gameName, tagLine, region)))
        .ToIActionResult(this);
}