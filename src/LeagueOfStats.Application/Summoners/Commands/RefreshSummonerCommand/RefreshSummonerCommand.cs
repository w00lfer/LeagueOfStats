using LeagueOfStats.Application.ApiClients.RiotClient;
using LeagueOfStats.Application.Common;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Domain.Summoners.Dtos;
using MediatR;
using NodaTime;
using Duration = NodaTime.Duration;

namespace LeagueOfStats.Application.Summoners.Commands.RefreshSummonerCommand;

public record RefreshSummonerCommand(
    Guid Id)
    : IRequest<Result>;

public class RefreshSummonerCommandHandler : IRequestHandler<RefreshSummonerCommand, Result>
{
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IRiotClient _riotClient;
    private readonly IEntityUpdateLockoutService _entityUpdateLockoutService;
    private readonly IClock _clock;
    private readonly IChampionRepository _championRepository;


    public RefreshSummonerCommandHandler(
        ISummonerDomainService summonerDomainService,
        IRiotClient riotClient,
        IEntityUpdateLockoutService entityUpdateLockoutService,
        IClock clock,
        IChampionRepository championRepository)
    {
        _summonerDomainService = summonerDomainService;
        _riotClient = riotClient;
        _entityUpdateLockoutService = entityUpdateLockoutService;
        _clock = clock;
        _championRepository = championRepository;
    }

    public Task<Result> Handle(RefreshSummonerCommand command, CancellationToken cancellationToken) =>
        _summonerDomainService.GetByIdAsync(command.Id)
            .Bind(summoner =>
                CanSummonerCanBeUpdatedWithRiotData(summoner)
                    ? UpdateSummonerDataWithDataFromRiotApiAsync(summoner)
                    : Task.FromResult(Result.Failure(
                        new ApplicationError($"Could not update summoner. Try refresh data on: {summoner.LastUpdated.Plus(Duration.FromMinutes(2))}."))));
    
    private bool CanSummonerCanBeUpdatedWithRiotData(Summoner summoner) =>
        GetSummonerMinutesFromLastUpdate(summoner) >= _entityUpdateLockoutService.GetSummonerUpdateLockoutInMinutes();

    private double GetSummonerMinutesFromLastUpdate(Summoner summoner) => 
        _clock.GetCurrentInstant().Minus(summoner.LastUpdated).TotalMinutes;

    private Task<Result> UpdateSummonerDataWithDataFromRiotApiAsync(Summoner summoner) =>
        _riotClient.GetSummonerByPuuidAsync(summoner.Puuid, summoner.Region)
            .Bind(summonerFromRiotApi => _riotClient
                .GetSummonerChampionMasteryByPuuid(summonerFromRiotApi.Puuid, summoner.Region)
                .Tap(async summonerChampionMasteriesFromRiotApi =>
                {
                    var champions = (await _championRepository.GetAllAsync()).ToList();
                    
                    await _summonerDomainService.UpdateDetailsAsync(
                        summoner,
                        new UpdateDetailsSummonerDto(
                            summonerFromRiotApi.ProfileIconId,
                            summonerFromRiotApi.SummonerLevel,
                            summonerChampionMasteriesFromRiotApi.Select(cm =>
                                new UpdateChampionMasteryDto(
                                    champions.Single(c => c.RiotChampionId == (int)cm.ChampionId),
                                    cm.ChampionLevel,
                                    cm.ChampionPoints,
                                    cm.ChampionPointsSinceLastLevel,
                                    cm.ChampionPointsUntilNextLevel,
                                    cm.ChestGranted,
                                    cm.LastPlayTime,
                                    cm.TokensEarned))));
                }))
            .ToNonValueResult();
}