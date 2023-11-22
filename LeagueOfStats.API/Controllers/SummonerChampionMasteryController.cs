using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.SummonerChampionMastery.Queries.GetSummonerChampionMastery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SummonerChampionMasteryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SummonerChampionMasteryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string server, string summonerName)
        {
            // var r = await _mediator.Send(new GetSummonerChampionMasteryRequest(server, summonerName));
            // r.Match()

            return (await _mediator.Send(new GetSummonerChampionMasteryRequest(server, summonerName)))
                .ToIActionResult(this);
        }
    }
}