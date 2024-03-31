using Camille.Enums;
using Camille.RiotGames.AccountV1;
using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.MatchV5;
using Camille.RiotGames.SpectatorV4;
using Camille.RiotGames.SummonerV4;

namespace LeagueOfStats.API.Infrastructure.ApiClients.RiotClient;

public interface IRiotGamesApiWrapper
{
    Task<Summoner?> GetSummonerByPuuidAsync(
        PlatformRoute platformRoute,
        string puuid,
        CancellationToken? cancellationToken = null);

    Task<Account?> GetAccountByRiotIdAsync(
        RegionalRoute regionalRoute,
        string gameName,
        string tagLine,
        CancellationToken? cancellationToken = null);
    
    Task<ChampionMastery[]> GetChampionMasteryByPuuid(
        PlatformRoute platformRoute,
        string puuid,
        CancellationToken? cancellationToken = null);

    Task<string[]> GetMatchIdsByPuuidAsync(
        RegionalRoute regionalRoute,
        string puuid,
        int? count = null,
        long? endTime = null,
        Queue? queue = null,
        long? startTime = null,
        int? start = null,
        string? type = null,
        CancellationToken? cancellationToken = null);

    Task<Match?> GetMatchByIdAsync(
        RegionalRoute regionalRoute,
        string matchId,
        CancellationToken? cancellationToken = null);

    Task<CurrentGameInfo?> GetCurrentGameInfoBySummonerIdAsync(
        PlatformRoute platformRoute,
        string summonerId,
        CancellationToken? cancellationToken = null);
}