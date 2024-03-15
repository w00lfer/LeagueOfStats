using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Application.ApiClients.CommunityDragonClient;

public interface ICommunityDragonClient
{
    Task<Result<IEnumerable<SkinDto>>> GetSkinsAsync();
}