using LeagueOfStats.Application.ApiClients.DataDragonClient;
using LeagueOfStats.Jobs.ApiClients.DataDragonClient;

namespace LeagueOfStats.API.Infrastructure.ApiClients.DataDragonClient;

public static class DataDragonClientConfiguration
{
    private const string DataDragonUrl = "https://ddragon.leagueoflegends.com/";
    
    public static void ConfigureDataDragonClient(this IServiceCollection services)
    {
        services.AddHttpClient<IDataDragonClient, DataDragonClient>(client =>
        {
            client.BaseAddress = new Uri(DataDragonUrl);
        });
    }
}