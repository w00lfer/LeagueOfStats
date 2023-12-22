using Camille.RiotGames;
using Camille.RiotGames.AccountV1;
using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.SummonerV4;
using Camille.RiotGames.Util;
using LanguageExt;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.API.Options;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Rails.Errors;
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
        Summoner? summoner;
        try
        { 
            summoner = await _riotGamesApi.SummonerV4().GetByPUUIDAsync(region.ToPlatformRoute(), puuid);
        }
        catch (RiotResponseException riotResponseException)
        {
            _logger.LogError(riotResponseException.ToString());
            return new ApiError("There are problems on Riot API side.");
        }

        return summoner is not null
            ? summoner
            : new ApiError($"Summoner with Puuid={puuid} and Region={region.ToString()} does not exist.");
    }

    public async Task<Result<Summoner>> GetSummonerByGameNameAndTaglineAsync(string gameName, string tagLine, Region region)
    {
        Summoner? summoner;
        try
        {
            // no need to use region here. just use closes cluster to server
            Account? account = await _riotGamesApi.AccountV1().GetByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine);

            if (account is null)
            {
                return new ApiError($"There is no such account: {gameName}#{tagLine}");
            }

            summoner = await _riotGamesApi.SummonerV4().GetByPUUIDAsync(region.ToPlatformRoute(), account.Puuid);
        }
        catch (RiotResponseException riotResponseException)
        {
            _logger.LogError(riotResponseException.ToString());
            return new ApiError("There are problems on Riot API side.");
        }
        
        return summoner is not null
            ? summoner
            : new ApiError($"Summoner with RiotId={gameName}#{tagLine} and Region={region.ToString()} does not exist.");
    }

    public async Task<Result<ChampionMastery[]>> GetSummonerChampionMasteryByPuuid(string puuid, Region region)
    {
        ChampionMastery[] championMasteries;
        try
        {
            championMasteries = await _riotGamesApi.ChampionMasteryV4().GetAllChampionMasteriesByPUUIDAsync(region.ToPlatformRoute(), puuid);
        }
        catch (RiotResponseException riotResponseException)
        {
            _logger.LogError(riotResponseException.ToString());
            return new ApiError("There are problems on Riot API side");
        }


        // There must be always champion mastery for existing player. So either PUUID is invalid or there were other network problems with Camille
        return championMasteries is not null
            ? championMasteries
            : new ApiError($"Summoner with Puuid={puuid} and Region={region} neither does not exist or has no champion masteries.");
    }
        
    private RiotGamesApi GetConfiguredRiotGamesApi(string riotApiKey)
    {
        var config = new RiotGamesApiConfig.Builder(riotApiKey)
        {
                
        }.Build();
            
        return RiotGamesApi.NewInstance(config);
    }
}