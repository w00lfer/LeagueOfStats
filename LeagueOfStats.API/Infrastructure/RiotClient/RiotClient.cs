using Camille.RiotGames;
using Camille.RiotGames.AccountV1;
using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.SummonerV4;
using Camille.RiotGames.Util;
using LanguageExt;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.API.Infrastructure.Constants;
using LeagueOfStats.Application.Enums;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Errors;

namespace LeagueOfStats.API.Infrastructure.RiotClient;

public class RiotClient : IRiotClient
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RiotClient> _logger;
    private readonly RiotGamesApi _riotGamesApi;
        
    public RiotClient(
        ILogger<RiotClient> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _riotGamesApi = GetConfiguredRiotGamesApi();
    }

    public async Task<Either<Error, Summoner>> GetSummonerByPuuidAsync(string puuid, Region region)
    {
        Summoner? summoner;
        try
        { 
            summoner = await _riotGamesApi.SummonerV4().GetByPUUIDAsync(region.ToPlatformRoute(), puuid);
        }
        catch (RiotResponseException riotResponseException)
        {
            _logger.LogError(riotResponseException.ToString());
            return new ApiError("There are problems on Riot API side");
        }
            
        return summoner is null
            ? new ApiError("Summoner does not exist.")
            : Either<Error, Summoner>.Right(summoner);
    }

    public async Task<Either<Error, Summoner>> GetSummonerByGameNameAndTaglineAsync(string gameName, string tagLine, Region region)
    {
        Summoner summoner;
        try
        {
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
            return new ApiError("There are problems on Riot API side");
        }
        
        return Either<Error, Summoner>.Right(summoner);
    }

    public async Task<Either<Error, ChampionMastery[]>> GetChampionMasteryAsync(string puuid, Region region)
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
        return championMasteries is null
            ? new ApiError("Champion masteries for given summoner does not exist.")
            : Either<Error, ChampionMastery[]>.Right(championMasteries);
    }
        
    private RiotGamesApi GetConfiguredRiotGamesApi()
    {
        var config = new RiotGamesApiConfig.Builder(_configuration.GetRequiredSection(ConfigurationConstants.RiotApiKey).Value!)
        {
                
        }.Build();
            
        return RiotGamesApi.NewInstance(config);
    }
}