using System.Reflection;
using LeagueOfStats.Application.Summoners;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueOfStats.Application;

public static class DependencyInjection
{
    public static void AddApplicationDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}