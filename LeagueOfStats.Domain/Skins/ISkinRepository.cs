using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Repositories;

namespace LeagueOfStats.Domain.Skins;

public interface ISkinRepository : IAsyncReadOnlyRepository<Skin>
{
    Task<IEnumerable<Skin>> GetByRiotIdsAsync(int[] riotIds);
}