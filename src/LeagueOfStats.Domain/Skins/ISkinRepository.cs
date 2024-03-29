using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Repositories;

namespace LeagueOfStats.Domain.Skins;

public interface ISkinRepository : IAsyncRepository<Skin>
{
    Task<IEnumerable<Skin>> GetByRiotSkinIdsAsync(IEnumerable<int> riotSkinIds);
}