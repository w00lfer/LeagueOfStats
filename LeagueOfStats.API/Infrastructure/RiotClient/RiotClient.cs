using Camille.RiotGames;
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

        public RiotGamesApi GetClient() => _riotGamesApi;

        private RiotGamesApi GetConfiguredRiotGamesApi()
        {
            var config = new RiotGamesApiConfig.Builder(_configuration.GetRequiredSection(ConfigurationConstants.RiotApiKey).Value!)
            {
                
            }.Build();
            
            return RiotGamesApi.NewInstance(config);
        }
    }
}