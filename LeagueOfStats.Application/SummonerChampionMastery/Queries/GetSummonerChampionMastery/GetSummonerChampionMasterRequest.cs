using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Camille.Enums;
using Camille.RiotGames;
using Camille.RiotGames.ChampionMasteryV4;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Champions;
using MediatR;

namespace LeagueOfStats.Application.SummonerChampionMastery.Queries.GetSummonerChampionMastery
{
    public record GetSummonerChampionMasterRequest(
            string Server,
            string SummonerName)
        : IRequest<IEnumerable<ChampionMastery>>;

    public class GetSummonerChampionMasteryRequestHandler : IRequestHandler<GetSummonerChampionMasterRequest, IEnumerable<ChampionMastery>>
    {
        private readonly IRiotClient _riotClient;
        private readonly IChampionRepository _championRepository;

        public GetSummonerChampionMasteryRequestHandler(IRiotClient riotClient, IChampionRepository championRepository)
        {
            _riotClient = riotClient;
            _championRepository = championRepository;
        }

        public async Task<IEnumerable<ChampionMastery>> Handle(GetSummonerChampionMasterRequest request, CancellationToken cancellationToken)
        {
            var client = _riotClient.GetClient();
            var summoner = await client.SummonerV4().GetBySummonerNameAsync(Enum.Parse<PlatformRoute>(request.Server), request.SummonerName);

            var championMasteries = await client.ChampionMasteryV4().GetAllChampionMasteriesByPUUIDAsync(Enum.Parse<PlatformRoute>(request.Server), summoner.Puuid);

            var champions = _championRepository.GetAll();
            
            return championMasteries;
        }
    }
}