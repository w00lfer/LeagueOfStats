using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Summoners;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace LeagueOfStats.Domain;

public static class DependencyInjection
{
    public static void AddDomainDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IClock>(c => SystemClock.Instance);
        services.AddScoped<ISummonerDomainService, SummonerDomainService>();
        services.AddScoped<IMatchDomainService, MatchDomainService>();
    }
}