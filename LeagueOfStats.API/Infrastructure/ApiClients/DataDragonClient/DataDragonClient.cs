using LeagueOfStats.Application.ApiClients.DataDragonClient;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.API.Infrastructure.ApiClients.DataDragonClient;

public class DataDragonClient : IDataDragonClient
{
    private readonly HttpClient _httpClient;

    public DataDragonClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Result<IEnumerable<ChampionDto>>> GetChampions()
    {
        throw new NotImplementedException();
    }
}