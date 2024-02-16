using LeagueOfStats.Application.Discounts.RiotGamesShopClient;

namespace LeagueOfStats.API.Infrastructure.RiotGamesShopClient;

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