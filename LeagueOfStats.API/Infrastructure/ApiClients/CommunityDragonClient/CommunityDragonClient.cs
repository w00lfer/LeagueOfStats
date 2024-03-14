using LeagueOfStats.Application.ApiClients.CommunityDragonClient;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.API.Infrastructure.ApiClients.CommunityDragonClient;

public class CommunityDragonClient : ICommunityDragonClient
{
    private readonly HttpClient _httpClient;

    public CommunityDragonClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Result<IEnumerable<SkinDto>>> GetSkins()
    {
        throw new NotImplementedException();
    }
}