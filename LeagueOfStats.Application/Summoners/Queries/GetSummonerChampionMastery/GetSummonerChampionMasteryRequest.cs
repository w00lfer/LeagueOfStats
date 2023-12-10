using Camille.RiotGames.ChampionMasteryV4;
using LanguageExt;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Errors;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMastery;

public record GetSummonerChampionMasteryRequest(
        string Puuid,
        Region Region)
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
        _riotClient.GetSummonerByPuuidAsync(request.Puuid, request.Region)
            .BindAsync(summoner => _riotClient.GetChampionMasteryAsync(summoner.Puuid, request.Region))
            .BindAsync(MapToSummonerChampionMasteryDtos);
    
    private async Task<Either<Error, IEnumerable<SummonerChampionMasteryDto>>> MapToSummonerChampionMasteryDtos(ChampionMastery[] championMasteries)
    {
        var championMasteriesByChampionId = championMasteries.ToDictionary(championMastery => (int)championMastery.ChampionId, championMastery => championMastery);

        var championsByChampionId = (await _championRepository.GetAllAsync()).ToDictionary(champion => champion.Id, champion => champion);

        if (championMasteriesByChampionId.Keys.All(c => championsByChampionId.Keys.Select(k => k.Value).Contains(c)) is false)
        {
            return new ApplicationError("Champions from masteries differ than champions from domain");
        }

        var summonerChampionMasteryDtos = championMasteriesByChampionId.Select(championMasteryByChampionId =>
        {
            championsByChampionId.TryGetValue(new ChampionId(championMasteryByChampionId.Key), out var champion);
            var championMastery = championMasteryByChampionId.Value;

            return new SummonerChampionMasteryDto(
                champion!.Id.Value,
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