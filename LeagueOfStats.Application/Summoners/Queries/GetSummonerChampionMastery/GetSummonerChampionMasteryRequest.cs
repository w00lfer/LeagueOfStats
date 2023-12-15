using LanguageExt;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Errors;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMastery;

public record GetSummonerChampionMasteryRequest(
    Guid Id)
: IRequest<Either<Error, IEnumerable<SummonerChampionMasteryDto>>>;

public class GetSummonerChampionMasteryRequestHandler : IRequestHandler<GetSummonerChampionMasteryRequest, Either<Error, IEnumerable<SummonerChampionMasteryDto>>>
{
    private readonly ISummonerApplicationService _summonerApplicationService;
    private readonly IRiotClient _riotClient;
    private readonly IChampionRepository _championRepository;

    public GetSummonerChampionMasteryRequestHandler(
        ISummonerApplicationService summonerApplicationService,
        IChampionRepository championRepository, IRiotClient riotClient)
    { 
        _summonerApplicationService = summonerApplicationService;
        _championRepository = championRepository;
        _riotClient = riotClient;
    }

    public Task<Either<Error, IEnumerable<SummonerChampionMasteryDto>>> Handle(GetSummonerChampionMasteryRequest request, CancellationToken cancellationToken) =>
        _summonerApplicationService.GetSummonerChampionMasteriesBySummonerId(request.Id)
            .BindAsync(MapToSummonerChampionMasteryDtos);
    
    private async Task<Either<Error, IEnumerable<SummonerChampionMasteryDto>>> MapToSummonerChampionMasteryDtos(IEnumerable<SummonerChampionMastery> summonerChampionMasteries)
    {
        var championMasteriesByRiotChampionId = summonerChampionMasteries.ToDictionary(championMastery => (int)championMastery.RiotChampionId, championMastery => championMastery);

        var championsByRiotChampionId = (await _championRepository.GetAllAsync()).ToDictionary(champion => champion.RiotChampionId, champion => champion);

        if (championMasteriesByRiotChampionId.Keys.All(c => championsByRiotChampionId.Keys.Select(k => k).Contains(c)) is false)
        {
            return new ApplicationError("Champions from masteries differ than champions from domain");
        }

        var summonerChampionMasteryDtos = championMasteriesByRiotChampionId.Select(championMasteryByRiotChampionId =>
        {
            championsByRiotChampionId.TryGetValue(championMasteryByRiotChampionId.Key, out var champion);
            var championMastery = championMasteryByRiotChampionId.Value;

            return new SummonerChampionMasteryDto(
                champion!.RiotChampionId,
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