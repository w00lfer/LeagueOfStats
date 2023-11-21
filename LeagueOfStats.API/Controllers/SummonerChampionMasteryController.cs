using CSharpFunctionalExtensions;
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
        public async Task<IActionResult> Get(string server, string summonerName) =>
            (await _mediator.Send(new GetSummonerChampionMasteryRequest(server, summonerName)))
            .Match(r => Ok(r), () => BadRequest("There was an error") as IActionResult);
    }
}