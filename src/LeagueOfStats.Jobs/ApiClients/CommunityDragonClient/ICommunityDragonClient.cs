using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Jobs.ApiClients.CommunityDragonClient;

public interface ICommunityDragonClient
{
    Task<Result<IEnumerable<SkinDto>>> GetSkinsAsync();
}