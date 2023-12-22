using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMastery;

public record GetSummonerChampionMasteryQuery(
    Guid SummonerId)
: IRequest<Result<IEnumerable<SummonerChampionMasteryDto>>>;

public class GetSummonerChampionMasteryQueryHandler : IRequestHandler<GetSummonerChampionMasteryQuery, Result<IEnumerable<SummonerChampionMasteryDto>>>
{
    private readonly IValidator<GetSummonerChampionMasteryQuery> _getSummonerChampionMasteryQueryValidator;
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IChampionRepository _championRepository;

    public GetSummonerChampionMasteryQueryHandler(
        IValidator<GetSummonerChampionMasteryQuery> getSummonerChampionMasteryQueryValidator,
        ISummonerDomainService summonerDomainService,
        IChampionRepository championRepository)
    { 
        _getSummonerChampionMasteryQueryValidator = getSummonerChampionMasteryQueryValidator;
        _summonerDomainService = summonerDomainService;
        _championRepository = championRepository;
    }

    public Task<Result<IEnumerable<SummonerChampionMasteryDto>>> Handle(GetSummonerChampionMasteryQuery query, CancellationToken cancellationToken) =>
        _getSummonerChampionMasteryQueryValidator.ValidateAsyncTwo(query)
            .Bind(() => _summonerDomainService.GetByIdAsyncTwo(query.SummonerId))
            .Bind(summoner => MapToSummonerChampionMasteryDtos(summoner.SummonerChampionMasteries));
    
    private async Task<Result<IEnumerable<SummonerChampionMasteryDto>>> MapToSummonerChampionMasteryDtos(IEnumerable<SummonerChampionMastery> summonerChampionMasteries)
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

        return Result.Success(summonerChampionMasteryDtos);
    }
}