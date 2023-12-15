using LanguageExt;
using LeagueOfStats.Application.Common;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Errors;
using LeagueOfStats.Domain.Summoners;
using NodaTime;

namespace LeagueOfStats.Application.Summoners;

public class SummonerApplicationService : ISummonerApplicationService
{
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IRiotClient _riotClient;
    private readonly IEntityUpdateLockoutService _entityUpdateLockoutService;
    private readonly IClock _clock;

    public SummonerApplicationService(
        ISummonerDomainService summonerDomainService,
        IRiotClient riotClient,
        IEntityUpdateLockoutService entityUpdateLockoutService,
        IClock clock)
    {
        _summonerDomainService = summonerDomainService;
        _riotClient = riotClient;
        _entityUpdateLockoutService = entityUpdateLockoutService;
        _clock = clock;
    }
    
    public Task<Either<Error, Summoner>> GetSummonerByGameNameAndTagLineAndRegion(string gameName, string tagLine, Region region) =>
        _riotClient.GetSummonerByGameNameAndTaglineAsync(gameName, tagLine, region)
            .BindAsync(async summonerFromRiotApi => await (await _summonerDomainService.GetByPuuidAsync(summonerFromRiotApi.Puuid))
                .MatchAsync(
                    summoner => Either<Error, Summoner>.Right(summoner),
                    async _ =>
                    {
                        var summoner = await _summonerDomainService.CreateAsync(new CreateSummonerDto(
                            summonerFromRiotApi.Id,
                            summonerFromRiotApi.AccountId,
                            summonerFromRiotApi.Name,
                            summonerFromRiotApi.ProfileIconId,
                            summonerFromRiotApi.Puuid,
                            summonerFromRiotApi.SummonerLevel,
                            gameName,
                            tagLine,
                            region));

                        return Either<Error, Summoner>.Right(summoner);
                    }));

    public Task<Either<Error, Summoner>> GetSummonerById(Guid id) =>
        _summonerDomainService.GetByIdAsync(id);
    
    public Task<Either<Error, IEnumerable<SummonerChampionMastery>>> GetSummonerChampionMasteriesBySummonerId(Guid summonerId) =>
        _summonerDomainService.GetByIdAsync(summonerId)
            .BindAsync(summoner => Either<Error, IEnumerable<SummonerChampionMastery>>.Right(summoner.SummonerChampionMasteries));

    public Task<Option<Error>> RefreshSummonerDataBySummonerId(Guid summonerId) =>
        _summonerDomainService.GetByIdAsync(summonerId)
            .ToAsync()
            .MatchAsync(
                summoner => 
                    CanSummonerCanBeUpdatedWithRiotData(summoner)
                        ? UpdateSummonerDataWithDataFromRiotApiAsync(summoner)
                        : Task.FromResult(Option<Error>.None),
                error => Option<Error>.Some(error));

    private bool CanSummonerCanBeUpdatedWithRiotData(Summoner summoner) =>
        _clock.GetCurrentInstant().Minus(summoner.LastUpdated).TotalMinutes >= _entityUpdateLockoutService.GetSummonerUpdateLockoutInMinutes();
    
    private Task<Option<Error>> UpdateSummonerDataWithDataFromRiotApiAsync(Summoner summoner) =>
        _riotClient.GetSummonerByPuuidAsync(summoner.Puuid, summoner.Region)
            .BindAsync(summonerFromRiotApi => _riotClient.GetSummonerChampionMasteryByPuuid(summonerFromRiotApi.Puuid, summoner.Region)
                .BindAsync(async summonerChampionMasteriesFromRiotApi =>
                {
                    await _summonerDomainService.UpdateDetailsAsync(
                        summoner,
                        new UpdateDetailsSummonerDto(summonerFromRiotApi.ProfileIconId,
                            summonerFromRiotApi.SummonerLevel));

                    await _summonerDomainService.UpdateChampionMasteriesAsync(
                        summoner,
                        summonerChampionMasteriesFromRiotApi.Select(c =>
                            new UpdateChampionMasteryDto(
                                (int)c.ChampionId,
                                c.ChampionLevel,
                                c.ChampionPoints,
                                c.ChampionPointsSinceLastLevel,
                                c.ChampionPointsUntilNextLevel,
                                c.ChestGranted,
                                c.LastPlayTime,
                                c.Puuid,
                                c.SummonerId,
                                c.TokensEarned)));

                    return Either<Error, IEnumerable<SummonerChampionMastery>>.Right(summoner.SummonerChampionMasteries);
                }))
            .ToAsync()
            .Match(
                _ => Option<Error>.None,
                error => Option<Error>.Some(error));
}