using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Infrastructure.Champions;
using LeagueOfStats.Infrastructure.Matches;
using LeagueOfStats.Infrastructure.Summoners;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueOfStats.Infrastructure;

    public static class DependencyInjection
    {
        public static void AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IChampionRepository, ChampionRepository>();
            
            // #TODO Move to Scoped when moving from InMemory db to real db
            services.AddSingleton<ISummonerRepository, SummonerRepository>();
            services.AddSingleton<IMatchRepository, MatchRepository>();
        }
    }