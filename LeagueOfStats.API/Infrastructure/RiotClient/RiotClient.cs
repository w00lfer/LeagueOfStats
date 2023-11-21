using Camille.Enums;
using Camille.RiotGames;
using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.SummonerV4;
using CSharpFunctionalExtensions;
using LeagueOfStats.API.Infrastructure.Constants;
using LeagueOfStats.Application.RiotClient;

namespace LeagueOfStats.API.Infrastructure.RiotClient
{
    public class RiotClient : IRiotClient
    {
        private readonly IConfiguration _configuration;
        private readonly RiotGamesApi _riotGamesApi;
        
        public RiotClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _riotGamesApi = GetConfiguredRiotGamesApi();
        }

        public async Task<Maybe<Summoner>> GetSummonerAsync(string server, string summonerName)
        {
            bool doesPlatformRouteExist = Enum.TryParse(server, out PlatformRoute platformRoute);

            if (doesPlatformRouteExist is false)
                return Maybe.None;
            
            Summoner? summoner = await _riotGamesApi.SummonerV4().GetBySummonerNameAsync(platformRoute, summonerName);
            
            return summoner is null
                ? Maybe.None
                : Maybe.From(summoner);
        }

        public async Task<Maybe<ChampionMastery[]>> GetChampionMasteryAsync(string server, string puuid)
        {
            bool doesPlatformRouteExist = Enum.TryParse(server, out PlatformRoute platformRoute);

            if (doesPlatformRouteExist is false)
                return Maybe.None;
            
            ChampionMastery[] championMastery = await _riotGamesApi.ChampionMasteryV4().GetAllChampionMasteriesByPUUIDAsync(platformRoute, puuid);

            // There must be always champion mastery for existing player. So either PUUID is invalid or there were other network problems with Camille
            return championMastery is null || championMastery.Length == 0
                ? Maybe.None
                : Maybe.From(championMastery);
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