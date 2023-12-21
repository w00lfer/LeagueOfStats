using LanguageExt;
using LeagueOfStats.Application.Common;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Errors;
using LeagueOfStats.Domain.Summoners;
using MediatR;
using NodaTime;
using Duration = NodaTime.Duration;

namespace LeagueOfStats.Application.Summoners.Commands.RefreshSummonerCommand;

public record RefreshSummonerCommand
    (Guid Id)
: IRequest<Option<Error>>;

public class RefreshSummonerCommandHandler : IRequestHandler<RefreshSummonerCommand, Option<Error>>
{
    private readonly IValidator<RefreshSummonerCommand> _refreshSummonerCommandValidator;
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IRiotClient _riotClient;
    private readonly IEntityUpdateLockoutService _entityUpdateLockoutService;
    private readonly IClock _clock;


    public RefreshSummonerCommandHandler(
        IValidator<RefreshSummonerCommand> refreshSummonerCommandValidator,
        ISummonerDomainService summonerDomainService,
        IRiotClient riotClient,
        IEntityUpdateLockoutService entityUpdateLockoutService,
        IClock clock)
    {
        _refreshSummonerCommandValidator = refreshSummonerCommandValidator;
        _summonerDomainService = summonerDomainService;
        _riotClient = riotClient;
        _entityUpdateLockoutService = entityUpdateLockoutService;
        _clock = clock;
    }

    public Task<Option<Error>> Handle(RefreshSummonerCommand request, CancellationToken cancellationToken) =>
        _refreshSummonerCommandValidator.ValidateAsync(request)
            .MatchAsync(
                error => error,
                () => _summonerDomainService.GetByIdAsync(request.Id).ToAsync()
                    .MatchAsync(
                        summoner =>
                            CanSummonerCanBeUpdatedWithRiotData(summoner)
                            ? UpdateSummonerDataWithDataFromRiotApiAsync(summoner)
                            : Task.FromResult(Option<Error>.Some(new ApplicationError($"Could not update summoner. Try refresh data on: {summoner.LastUpdated.Plus(Duration.FromMinutes(2))}."))),
                    error => Option<Error>.Some(error)));

    private bool CanSummonerCanBeUpdatedWithRiotData(Summoner summoner) =>
        _clock.GetCurrentInstant().Minus(summoner.LastUpdated).TotalMinutes >= _entityUpdateLockoutService.GetSummonerUpdateLockoutInMinutes();
    
    private Task<Option<Error>> UpdateSummonerDataWithDataFromRiotApiAsync(Summoner summoner) =>
        _riotClient.GetSummonerByPuuidAsync(summoner.Puuid, summoner.Region)
            .BindAsync(summonerFromRiotApi => _riotClient.GetSummonerChampionMasteryByPuuid(summonerFromRiotApi.Puuid, summoner.Region)
                .BindAsync(async summonerChampionMasteriesFromRiotApi =>
                {
                    await _summonerDomainService.UpdateDetailsAsync(
                        summoner,
                        new UpdateDetailsSummonerDto(
                            summonerFromRiotApi.ProfileIconId,
                            summonerFromRiotApi.SummonerLevel,
                            summonerChampionMasteriesFromRiotApi.Select(c =>
                                new UpdateChampionMasteryDto(
                                    (int)c.ChampionId,
                                    c.ChampionLevel,
                                    c.ChampionPoints,
                                    c.ChampionPointsSinceLastLevel,
                                    c.ChampionPointsUntilNextLevel,
                                    c.ChestGranted,
                                    c.LastPlayTime,
                                    c.TokensEarned))));

                    return Either<Error, IEnumerable<SummonerChampionMastery>>.Right(summoner.SummonerChampionMasteries);
                }))
            .ToAsync()
            .Match(
                _ => Option<Error>.None,
                error => Option<Error>.Some(error));
    
}