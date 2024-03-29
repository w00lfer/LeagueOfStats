using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using LeagueOfStats.Infrastructure.Common;

namespace LeagueOfStats.Infrastructure.Champions;

public class ChampionRepository : AsyncRepositoryBase<Champion>, IChampionRepository
{
    public ChampionRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext)
    {
    }
}