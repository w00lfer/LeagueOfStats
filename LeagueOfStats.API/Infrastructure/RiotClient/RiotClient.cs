using Camille.RiotGames;
using Camille.RiotGames.AccountV1;
using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.MatchV5;
using Camille.RiotGames.SpectatorV4;
using Camille.RiotGames.SummonerV4;
using Camille.RiotGames.Util;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.API.Options;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Rails.Results;
using Microsoft.Extensions.Options;

namespace LeagueOfStats.API.Infrastructure.RiotClient;

public class RiotClient : IRiotClient
{
    private readonly ILogger<RiotClient> _logger;
    private readonly RiotGamesApi _riotGamesApi;
        
    public RiotClient(
        ILogger<RiotClient> logger,
        IOptionsSnapshot<RiotApiKeyOptions> riotApiKeyOptionsSnapshot)
    {
        _logger = logger;
        _riotGamesApi = GetConfiguredRiotGamesApi(riotApiKeyOptionsSnapshot.Value.RiotApiKey);
    }

    public async Task<Result<Summoner>> GetSummonerByPuuidAsync(string puuid, Region region)
    {
        try
        { 
            Summoner? summoner = await _riotGamesApi
                .SummonerV4()
                .GetByPUUIDAsync(region.ToPlatformRoute(), puuid);
            
            return summoner is not null
                ? summoner
                : new ApiError($"Summoner with Puuid={puuid} and Region={region.ToString()} does not exist.");
        }
        catch (RiotResponseException riotResponseException)
        {
            _logger.LogError(riotResponseException.ToString());
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
            Account? account = await _riotGamesApi
                .AccountV1()
                .GetByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine);

            if (account is null)
            {
                return new ApiError($"There is no such account: {gameName}#{tagLine}");
            }

            Summoner? summoner = await _riotGamesApi
                .SummonerV4()
                .GetByPUUIDAsync(region.ToPlatformRoute(), account.Puuid);
            
            return summoner is not null
                ? summoner
                : new ApiError($"Summoner with RiotId={gameName}#{tagLine} and Region={region.ToString()} does not exist.");
        }
        catch (RiotResponseException riotResponseException)
        {
            _logger.LogError(riotResponseException.ToString());
            return new ApiError("There are problems on Riot API side.");
        }
    }

    public async Task<Result<ChampionMastery[]>> GetSummonerChampionMasteryByPuuid(
        string puuid,
        Region region)
    {
        ChampionMastery[] championMasteries;
        try
        {
            championMasteries = await _riotGamesApi
                .ChampionMasteryV4()
                .GetAllChampionMasteriesByPUUIDAsync(region.ToPlatformRoute(), puuid);
            
            return championMasteries is not null
                ? championMasteries
                : new ApiError($"Summoner with Puuid={puuid} and Region={region} neither does not exist or has no champion masteries.");
        }
        catch (RiotResponseException riotResponseException)
        {
            _logger.LogError(riotResponseException.ToString());
            return new ApiError("There are problems on Riot API side");
        }
    }

    public async Task<Result<IEnumerable<Match>>> GetSummonerMatchHistorySummary(
        GetSummonerMatchHistoryDto getSummonerMatchHistoryDto) 
    {
        try
        {
            var matchHistoryIds = await _riotGamesApi
                .MatchV5()
                .GetMatchIdsByPUUIDAsync(
                    getSummonerMatchHistoryDto.Region.ToRegionalRoute(),
                    getSummonerMatchHistoryDto.Puuid,
                    getSummonerMatchHistoryDto.Count,
                    getSummonerMatchHistoryDto.GameEndedAt.ToUnixTimeSeconds(),
                    getSummonerMatchHistoryDto.QueueFilter.ToNullableQueue());

            var matches = await Task.WhenAll(
                matchHistoryIds.Select(mId => 
                    _riotGamesApi
                        .MatchV5()
                        .GetMatchAsync(getSummonerMatchHistoryDto.Region.ToRegionalRoute(), mId)));

            return Result.Success<IEnumerable<Match>>(matches.Where(m => m is not null));
        }
        catch (RiotResponseException riotResponseException)
        {
            _logger.LogError(riotResponseException.ToString());
            return new ApiError("There are problems on Riot API side");
        }
    }

    public async Task<Result<CurrentGameInfo?>> GetSummonerLiveGame(
        GetSummonerLiveGameDto getSummonerLiveGameDto)
    {
        try
        {
            CurrentGameInfo? liveGame = await _riotGamesApi
                .SpectatorV4()
                .GetCurrentGameInfoBySummonerAsync(
                    getSummonerLiveGameDto.Region.ToPlatformRoute(),
                    getSummonerLiveGameDto.RiotSummonerId);

            return liveGame is not null
                ? liveGame
                : new ApiError($"Summoner={getSummonerLiveGameDto.SummonerName} is not in game.");;
        }
        catch (RiotResponseException riotResponseException)
        {
            _logger.LogError(riotResponseException.ToString());
            return new ApiError("There are problems on Riot API side");
        }
    }
        
    private RiotGamesApi GetConfiguredRiotGamesApi(string riotApiKey)
    {
        var config = new RiotGamesApiConfig.Builder(riotApiKey)
        {
                
        }.Build();
            
        return RiotGamesApi.NewInstance(config);
    }
}