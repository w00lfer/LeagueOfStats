using LanguageExt;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Errors;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMastery;

public record GetSummonerChampionMasteryQuery(
    Guid SummonerId)
: IRequest<Either<Error, IEnumerable<SummonerChampionMasteryDto>>>;

public class GetSummonerChampionMasteryQueryHandler : IRequestHandler<GetSummonerChampionMasteryQuery, Either<Error, IEnumerable<SummonerChampionMasteryDto>>>
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

    public Task<Either<Error, IEnumerable<SummonerChampionMasteryDto>>> Handle(GetSummonerChampionMasteryQuery query, CancellationToken cancellationToken) =>
        _getSummonerChampionMasteryQueryValidator.ValidateAsync(query)
            .MatchAsync(
                error => error,
                () => _summonerDomainService.GetByIdAsync(query.SummonerId)
                    .BindAsync(summoner => MapToSummonerChampionMasteryDtos(summoner.SummonerChampionMasteries)));
    
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