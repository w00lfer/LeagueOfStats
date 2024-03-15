using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMasteries;

public record GetSummonerChampionMasteriesQuery(
    Guid SummonerId) 
    : IRequest<Result<IEnumerable<SummonerChampionMasteryDto>>>;

public class GetSummonerChampionMasteriesQueryHandler
    : IRequestHandler<GetSummonerChampionMasteriesQuery, Result<IEnumerable<SummonerChampionMasteryDto>>>
{
    private readonly IValidator<GetSummonerChampionMasteriesQuery> _getSummonerChampionMasteriesQueryValidator;
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IChampionRepository _championRepository;

    public GetSummonerChampionMasteriesQueryHandler(
        IValidator<GetSummonerChampionMasteriesQuery> getSummonerChampionMasteriesQueryValidator,
        ISummonerDomainService summonerDomainService,
        IChampionRepository championRepository)
    { 
        _getSummonerChampionMasteriesQueryValidator = getSummonerChampionMasteriesQueryValidator;
        _summonerDomainService = summonerDomainService;
        _championRepository = championRepository;
    }

    public Task<Result<IEnumerable<SummonerChampionMasteryDto>>> Handle(
        GetSummonerChampionMasteriesQuery query,
        CancellationToken cancellationToken) =>
        _getSummonerChampionMasteriesQueryValidator.ValidateAsync(query)
            .Bind(() => _summonerDomainService.GetByIdAsync(query.SummonerId))
            .Bind(summoner => MapToSummonerChampionMasteryDtos(summoner.SummonerChampionMasteries));
    
    private async Task<Result<IEnumerable<SummonerChampionMasteryDto>>> MapToSummonerChampionMasteryDtos(
        IEnumerable<SummonerChampionMastery> summonerChampionMasteries)
    {
        var championMasteriesByChampionId = summonerChampionMasteries
            .ToDictionary(championMastery => championMastery.ChampionId, championMastery => championMastery);

        var championsById = (await _championRepository.GetAllAsync())
            .ToDictionary(champion => champion.Id, champion => champion);

        if (championMasteriesByChampionId.Keys.All(cId => championsById.ContainsKey(cId)) is false)
        {
            return new ApplicationError("Champions from masteries differ than champions from domain.");
        }

        var summonerChampionMasteryDtos = championMasteriesByChampionId
            .Select(championMasteryByChampionId => 
            {
                var champion = championsById[championMasteryByChampionId.Key];
                var championMastery = championMasteryByChampionId.Value;

                return new SummonerChampionMasteryDto(
                    champion!.RiotChampionId,
                    champion.Name,
                    championMastery.ChampionLevel,
                    champion.Title,
                    champion.Description,
                    champion.ChampionImage.SplashUrl,
                    champion.ChampionImage.IconUrl,
                    championMastery.ChampionPoints,
                    championMastery.ChestGranted);
            });

        return Result.Success(summonerChampionMasteryDtos);
    }
}