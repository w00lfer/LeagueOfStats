using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Infrastructure.Champions;
using LeagueOfStats.Infrastructure.Summoners;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueOfStats.Infrastructure;

    public static class DependencyInjection
    {
        public static void AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IChampionRepository, ChampionRepository>();
            services.AddSingleton<ISummonerRepository, SummonerRepository>();
        }
    }