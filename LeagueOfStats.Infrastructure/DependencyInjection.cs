using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Infrastructure.Champions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueOfStats.Infrastructure;

    public static class DependencyInjection
    {
        public static void AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IChampionRepository, ChampionRepository>();
        }
    }