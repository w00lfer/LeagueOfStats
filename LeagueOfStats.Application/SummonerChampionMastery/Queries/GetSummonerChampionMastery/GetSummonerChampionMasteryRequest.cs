using Camille.RiotGames.ChampionMasteryV4;
using LanguageExt;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Errors;
using MediatR;

namespace LeagueOfStats.Application.SummonerChampionMastery.Queries.GetSummonerChampionMastery
{
    public record GetSummonerChampionMasteryRequest(
            string Server,
            string SummonerName)
        : IRequest<Either<Error, IEnumerable<SummonerChampionMasteryDto>>>;

    public class GetSummonerChampionMasteryRequestHandler : IRequestHandler<GetSummonerChampionMasteryRequest, Either<Error, IEnumerable<SummonerChampionMasteryDto>>>
    {
        private readonly IRiotClient _riotClient;
        private readonly IChampionRepository _championRepository;

        public GetSummonerChampionMasteryRequestHandler(IRiotClient riotClient, IChampionRepository championRepository)
        {
            _riotClient = riotClient;
            _championRepository = championRepository;
        }

        public Task<Either<Error, IEnumerable<SummonerChampionMasteryDto>>> Handle(GetSummonerChampionMasteryRequest request, CancellationToken cancellationToken) =>
            _riotClient.GetSummonerAsync(request.Server, request.SummonerName)
                .BindAsync(summoner => _riotClient.GetChampionMasteryAsync(request.Server, summoner.Puuid))
                .BindAsync(MapToSummonerChampionMasteryDtos);

        private Either<Error, IEnumerable<SummonerChampionMasteryDto>> MapToSummonerChampionMasteryDtos(ChampionMastery[] championMasteries)
        {
            var championMasteriesByChampionId = championMasteries.ToDictionary(championMastery => (int)championMastery.ChampionId, championMastery => championMastery);

            var championsByChampionId = _championRepository.GetAll().ToDictionary(champion => champion.Id, champion => champion);

            if (championMasteriesByChampionId.Keys.All(championsByChampionId.Keys.Contains) is false)
            {
                return new ApplicationError("Champions from masteries differ than champions from domain");
            }

            var summonerChampionMasteryDtos = championMasteriesByChampionId.Select(championMasteryByChampionId =>
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
            });

            return Either<Error, IEnumerable<SummonerChampionMasteryDto>>.Right(summonerChampionMasteryDtos);
        }
    }
}