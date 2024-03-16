using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using LeagueOfStats.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.Champions;

public class ChampionRepository : AsyncRepositoryBase<Champion>, IChampionRepository
{
    public ChampionRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext)
    {
    }

    public Task<Champion?> GetByIdAsync(Guid id) =>
        _applicationDbContext.Champions.SingleOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Champion>> GetAllAsync(params Guid[] ids) =>
        ids.Length > 0
            ? await _applicationDbContext.Champions.Where(c => ids.Contains(c.Id)).AsNoTracking().ToListAsync()
            : await _applicationDbContext.Champions.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Champion>> GetByRiotIdsAsync(int[] riotIds) =>
        riotIds.Length > 0
            ? await _applicationDbContext.Champions.Where(c => riotIds.Contains(c.RiotChampionId)).AsNoTracking().ToListAsync()
            : await _applicationDbContext.Champions.AsNoTracking().ToListAsync();
}