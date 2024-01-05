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
        var championMasteriesByRiotChampionId = summonerChampionMasteries
            .ToDictionary(championMastery => championMastery.RiotChampionId, championMastery => championMastery);

        var championsByRiotChampionId = (await _championRepository.GetAllAsync())
            .ToDictionary(champion => champion.RiotChampionId, champion => champion);

        if (championMasteriesByRiotChampionId.Keys.All(cId => championsByRiotChampionId.ContainsKey(cId)) is false)
        {
            return new ApplicationError("Champions from masteries differ than champions from domain.");
        }

        var summonerChampionMasteryDtos = championMasteriesByRiotChampionId
            .Select(championMasteryByRiotChampionId => 
            {
                var champion = championsByRiotChampionId[championMasteryByRiotChampionId.Key];
                var championMastery = championMasteryByRiotChampionId.Value;

                return new SummonerChampionMasteryDto(
                    champion!.RiotChampionId,
                    champion.Name,
                    championMastery.ChampionLevel,
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