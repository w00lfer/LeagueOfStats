using Camille.RiotGames.AccountV1;
using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.MatchV5;
using Camille.RiotGames.SpectatorV4;
using Camille.RiotGames.SummonerV4;
using Camille.RiotGames.Util;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.Application.ApiClients.RiotClient;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.API.Infrastructure.ApiClients.RiotClient;

public class RiotClient : IRiotClient
{
    private readonly IRiotGamesApiWrapper _riotGamesApiWrapper;
    
    public RiotClient(IRiotGamesApiWrapper riotGamesApiWrapper)
    {
        _riotGamesApiWrapper = riotGamesApiWrapper;
    }

    public async Task<Result<Summoner>> GetSummonerByPuuidAsync(string puuid, Region region)
    {
        try
        {
            Summoner? summoner = await _riotGamesApiWrapper.GetSummonerByPuuidAsync(
                region.ToPlatformRoute(),
                puuid);

            return summoner is not null
                ? summoner
                : new ApiError($"Summoner with Puuid={puuid} and Region={region.ToString()} does not exist.");
        }
        catch (RiotResponseException)
        {
            return new ApiError("There are problems on Riot API side.");
        }
    }

    public async Task<Result<Summoner>> GetSummonerByGameNameAndTaglineAsync(
        string gameName,
        string tagLine,
        Region region)
    {
        try
        {
            // no need to use region here. just use closes cluster to server
            Account? account = await _riotGamesApiWrapper.GetAccountByRiotIdAsync(
                region.ToRegionalRoute(),
                gameName,
                tagLine);

            if (account is null)
            {
                return new ApiError($"There is no such account: {gameName}#{tagLine}.");
            }

            Summoner? summoner = await _riotGamesApiWrapper.GetSummonerByPuuidAsync(
                region.ToPlatformRoute(),
                account.Puuid);

            return summoner is not null
                ? summoner
                : new ApiError($"Summoner with RiotId={gameName}#{tagLine} and Region={region.ToString()} does not exist.");
        }
        catch (RiotResponseException)
        {
            return new ApiError("There are problems on Riot API side.");
        }
    }

    public async Task<Result<ChampionMastery[]>> GetSummonerChampionMasteryByPuuidAsync(
        string puuid,
        Region region)
    {
        try
        {
            ChampionMastery[] championMasteries = await _riotGamesApiWrapper.GetChampionMasteryByPuuid(
                region.ToPlatformRoute(),
                puuid);

            return championMasteries is not null
                ? championMasteries
                : new ApiError($"Summoner with Puuid={puuid} and Region={region} neither does not exist or has no champion masteries.");
        }
        catch (RiotResponseException)
        {
            return new ApiError("There are problems on Riot API side.");
        }
    }

    public async Task<Result<IEnumerable<Match>>> GetSummonerMatchHistorySummaryAsync(
        GetSummonerMatchHistoryDto getSummonerMatchHistoryDto)
    {
        try
        {
            var matchHistoryIds = await _riotGamesApiWrapper.GetMatchIdsByPuuidAsync(
                getSummonerMatchHistoryDto.Region.ToRegionalRoute(),
                    getSummonerMatchHistoryDto.Puuid,
                    getSummonerMatchHistoryDto.Count,
                    getSummonerMatchHistoryDto.GameEndedAt.ToUnixTimeSeconds(),
                    getSummonerMatchHistoryDto.MatchHistoryQueueFilter.ToNullableQueue());

            var matches = await Task.WhenAll(
                matchHistoryIds.Select(matchId =>
                    _riotGamesApiWrapper.GetMatchByIdAsync(
                        getSummonerMatchHistoryDto.Region.ToRegionalRoute(),
                        matchId)));

            return Result.Success<IEnumerable<Match>>(matches.Where(m => m is not null));
        }
        catch (RiotResponseException)
        {
            return new ApiError("There are problems on Riot API side.");
        }
    }

    public async Task<Result<CurrentGameInfo>> GetSummonerLiveGameAsync(
        GetSummonerLiveGameDto getSummonerLiveGameDto)
    {
        try
        {
            CurrentGameInfo? liveGame = await _riotGamesApiWrapper.GetCurrentGameInfoBySummonerIdAsync(
                    getSummonerLiveGameDto.Region.ToPlatformRoute(),
                    getSummonerLiveGameDto.RiotSummonerId);

            return liveGame is not null
                ? liveGame
                : new ApiError($"Summoner={getSummonerLiveGameDto.SummonerName} is not in game.");;
        }
        catch (RiotResponseException)
        {
            return new ApiError("There are problems on Riot API side.");
        }
    }
}