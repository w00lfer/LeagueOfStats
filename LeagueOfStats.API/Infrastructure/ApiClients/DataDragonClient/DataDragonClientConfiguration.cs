using LeagueOfStats.Application.ApiClients.DataDragonClient;

namespace LeagueOfStats.API.Infrastructure.ApiClients.DataDragonClient;

public static class DataDragonClientConfiguration
{
    private const string DataDragonUrl = "https://ddragon.leagueoflegends.com/cdn/";
    
    public static void ConfigureDataDragonClient(this IServiceCollection services)
    {
        services.AddHttpClient<IDataDragonClient, DataDragonClient>(client =>
        {
            client.BaseAddress = new Uri(DataDragonUrl);
        });
    }
}