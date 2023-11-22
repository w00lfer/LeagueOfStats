using Camille.Enums;
using Camille.RiotGames;
using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.SummonerV4;
using Camille.RiotGames.Util;
using LanguageExt;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.API.Infrastructure.Constants;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Errors;

namespace LeagueOfStats.API.Infrastructure.RiotClient
{
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

        public async Task<Either<Error, Summoner>> GetSummonerAsync(string server, string summonerName)
        {
            bool doesPlatformRouteExist = Enum.TryParse(server, out PlatformRoute platformRoute);

            if (doesPlatformRouteExist is false)
                return new ApiError("Server name is invalid.");

            Summoner? summoner;
            try
            { 
                summoner = await _riotGamesApi.SummonerV4().GetBySummonerNameAsync(platformRoute, summonerName);
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

        public async Task<Either<Error, ChampionMastery[]>> GetChampionMasteryAsync(string server, string puuid)
        {
            bool doesPlatformRouteExist = Enum.TryParse(server, out PlatformRoute platformRoute);

            if (doesPlatformRouteExist is false)
                return new ApiError("Server name is invalid.");
            
            ChampionMastery[] championMastery;
            try
            {
                championMastery = await _riotGamesApi.ChampionMasteryV4().GetAllChampionMasteriesByPUUIDAsync(platformRoute, puuid);
            }
            catch (RiotResponseException riotResponseException)
            {
                _logger.LogError(riotResponseException.ToString());
                return new ApiError("There are problems on Riot API side");
            }


            // There must be always champion mastery for existing player. So either PUUID is invalid or there were other network problems with Camille
            return championMastery is null
                ? new ApiError("Champion masteries for given summoner does not exist.")
                : Either<Error, ChampionMastery[]>.Right(championMastery);
        }
        
        private RiotGamesApi GetConfiguredRiotGamesApi()
        {
            var config = new RiotGamesApiConfig.Builder(_configuration.GetRequiredSection(ConfigurationConstants.RiotApiKey).Value!)
            {
                
            }.Build();
            
            return RiotGamesApi.NewInstance(config);
        }
    }
}