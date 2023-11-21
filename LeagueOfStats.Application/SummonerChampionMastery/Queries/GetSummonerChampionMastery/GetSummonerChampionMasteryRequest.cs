using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.SummonerV4;
using CSharpFunctionalExtensions;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Champions;
using MediatR;
using Champion = LeagueOfStats.Domain.Champions.Champion;

namespace LeagueOfStats.Application.SummonerChampionMastery.Queries.GetSummonerChampionMastery
{
    public record GetSummonerChampionMasteryRequest(
            string Server,
            string SummonerName)
        : IRequest<Maybe<IEnumerable<SummonerChampionMasteryDto>>>;

    public class GetSummonerChampionMasteryRequestHandler : IRequestHandler<GetSummonerChampionMasteryRequest, Maybe<IEnumerable<SummonerChampionMasteryDto>>>
    {
        private readonly IRiotClient _riotClient;
        private readonly IChampionRepository _championRepository;

        public GetSummonerChampionMasteryRequestHandler(IRiotClient riotClient, IChampionRepository championRepository)
        {
            _riotClient = riotClient;
            _championRepository = championRepository;
        }

        public Task<Maybe<IEnumerable<SummonerChampionMasteryDto>>> Handle(GetSummonerChampionMasteryRequest request, CancellationToken cancellationToken) =>
            _riotClient.GetSummonerAsync(request.Server, request.SummonerName)
                .Bind(summoner => _riotClient.GetChampionMasteryAsync(request.Server, summoner.Puuid)
                    .Bind(MapToSummonerChampionMasteryDtos));

        private Maybe<IEnumerable<SummonerChampionMasteryDto>> MapToSummonerChampionMasteryDtos(ChampionMastery[] championMasteries)
        {
            var championMasteriesByChampionId = championMasteries.ToDictionary(championMastery => (int)championMastery.ChampionId, championMastery => championMastery);

            var championsByChampionId = _championRepository.GetAll().ToDictionary(champion => champion.Id, champion => champion);
            
            if (championMasteriesByChampionId.Keys.All(championsByChampionId.Keys.Contains) is false)
            {
                return Maybe.None;
            }


            return Maybe.From(championMasteriesByChampionId.Select(championMasteryByChampionId =>
            {
                Champion champion = championsByChampionId[championMasteryByChampionId.Key];
                ChampionMastery championMastery = championMasteryByChampionId.Value;

                return new SummonerChampionMasteryDto(
                    champion.Id,
                    champion.Name,
                    champion.Title,
                    champion.Description,
                    champion.ChampionImage.FullFileName,
                    champion.ChampionImage.SpriteFileName,
                    champion.ChampionImage.Height,
                    champion.ChampionImage.Width,
                    championMastery.ChampionPoints,
                    championMastery.ChestGranted);
            }));
        }
    }
}