using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Summoners.Commands.RefreshSummonerCommand;
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
    public Task<IActionResult> Get(Guid id) =>
        _mediator.Send(new GetSummonerByIdQuery(id))
            .ToIActionResult(this);
    
    [HttpGet("Search")]
    public Task<IActionResult> SearchByGameNameAndTagLineAndRegion([FromQuery] string gameName, [FromQuery] string tagLine, [FromQuery] Region region) => 
            _mediator.Send(new SearchSummonerByGameNameAndTagLineAndRegionQuery(gameName, tagLine, region))
            .ToIActionResult(this);
    
    [HttpPost("{id}/Refresh")]
    public Task<IActionResult> Refresh(Guid id) => 
        _mediator.Send(new RefreshSummonerCommand(id))
            .ToIActionResult(this);
}