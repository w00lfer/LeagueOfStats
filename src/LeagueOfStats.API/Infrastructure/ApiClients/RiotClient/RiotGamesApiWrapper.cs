using Camille.Enums;
using Camille.RiotGames;
using Camille.RiotGames.AccountV1;
using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.MatchV5;
using Camille.RiotGames.SpectatorV4;
using Camille.RiotGames.SummonerV4;
using LeagueOfStats.API.Configurations.Options;
using Microsoft.Extensions.Options;

namespace LeagueOfStats.API.Infrastructure.ApiClients.RiotClient;

internal sealed class RiotGamesApiWrapper : IRiotGamesApiWrapper
{
    private readonly RiotGamesApi _riotGamesApi;

    public RiotGamesApiWrapper(IOptionsSnapshot<RiotApiKeyOptions> riotApiKeyOptionsSnapshot)
    {
        _riotGamesApi = GetConfiguredRiotGamesApi(riotApiKeyOptionsSnapshot.Value.RiotApiKey);
    }
    
    public async Task<Summoner?> GetSummonerByPuuidAsync(
        PlatformRoute platformRoute,
        string puuid,
        CancellationToken? cancellationToken = null)
    {
        Summoner? summoner = await _riotGamesApi
            .SummonerV4()
            .GetByPUUIDAsync(platformRoute, puuid, cancellationToken);

        return summoner;
    }

    public async Task<Account?> GetAccountByRiotIdAsync(
        RegionalRoute regionalRoute,
        string gameName,
        string tagLine,
        CancellationToken? cancellationToken = null)
    {
        Account? account = await _riotGamesApi
            .AccountV1()
            .GetByRiotIdAsync(regionalRoute, gameName, tagLine, cancellationToken);

        return account;
    }

    public async Task<ChampionMastery[]> GetChampionMasteryByPuuid(
        PlatformRoute platformRoute,
        string puuid,
        CancellationToken? cancellationToken = null)
    {
        ChampionMastery[] championMasteries = await _riotGamesApi
            .ChampionMasteryV4()
            .GetAllChampionMasteriesByPUUIDAsync(platformRoute, puuid, cancellationToken);

        return championMasteries;
    }

    public async Task<string[]> GetMatchIdsByPuuidAsync(
        RegionalRoute regionalRoute,
        string puuid,
        int? count = null,
        long? endTime = null,
        Queue? queue = null,
        long? startTime = null,
        int? start = null,
        string? type = null,
        CancellationToken? cancellationToken = null)
    {
        string[] matchHistoryIds = await _riotGamesApi
            .MatchV5()
            .GetMatchIdsByPUUIDAsync(
                regionalRoute,
                puuid,
                count,
                endTime,
                queue,
                startTime,
                start,
                type,
                cancellationToken);

        return matchHistoryIds;
    }

    public async Task<Match?> GetMatchByIdAsync(
        RegionalRoute regionalRoute,
        string matchId,
        CancellationToken? cancellationToken = null)
    {
        Match? match = await _riotGamesApi
            .MatchV5()
            .GetMatchAsync(regionalRoute, matchId, cancellationToken);

        return match;
    }

    public async Task<CurrentGameInfo?> GetCurrentGameInfoBySummonerIdAsync(
        PlatformRoute platformRoute,
        string summonerId,
        CancellationToken? cancellationToken = null)
    {
        CurrentGameInfo? liveGame = await _riotGamesApi
            .SpectatorV4()
            .GetCurrentGameInfoBySummonerAsync(platformRoute, summonerId, cancellationToken);

        return liveGame;
    }
    
    private RiotGamesApi GetConfiguredRiotGamesApi(string riotApiKey)
    {
        var config = new RiotGamesApiConfig.Builder(riotApiKey)
            .Build();

        return RiotGamesApi.NewInstance(config);
    }
}