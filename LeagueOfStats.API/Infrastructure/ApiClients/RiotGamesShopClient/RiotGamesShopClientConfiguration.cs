using LeagueOfStats.Application.ApiClients.RiotGamesShopClient;

namespace LeagueOfStats.API.Infrastructure.ApiClients.RiotGamesShopClient;

public static class RiotGamesShopClientConfiguration
{
    private const string RiotGamesShopUrl = "https://api.shop.riotgames.com/v3/";
    
    public static void ConfigureRiotGamesShopClient(this IServiceCollection services)
    {
        services.AddHttpClient<IRiotGamesShopClient, RiotGamesShopClient>(client =>
        {
            client.BaseAddress = new Uri(RiotGamesShopUrl);
        });
    }
}