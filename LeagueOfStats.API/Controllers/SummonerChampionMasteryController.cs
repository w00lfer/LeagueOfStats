using System.Threading.Tasks;
using Camille.Enums;
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
            return Ok(await _mediator.Send(new GetSummonerChampionMasterRequest(server, summonerName)));
        }
    }
}