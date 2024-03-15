using LeagueOfStats.Application.ApiClients.CommunityDragonClient;
using LeagueOfStats.Jobs.ApiClients.CommunityDragonClient;

namespace LeagueOfStats.API.Infrastructure.ApiClients.CommunityDragonClient;

public static class CommunityDragonClientConfiguration
{
    private const string CommunityDragonUrl = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/";
    
    public static void ConfigureCommunityDragonClient(this IServiceCollection services)
    {
        services.AddHttpClient<ICommunityDragonClient, CommunityDragonClient>(client =>
        {
            client.BaseAddress = new Uri(CommunityDragonUrl);
        });
    }
}