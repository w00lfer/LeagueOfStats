using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.Champions;

public class ChampionRepository : IChampionRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public ChampionRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public Task<Champion?> GetByIdAsync(Guid id) =>
        _applicationDbContext.Champions.SingleOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Champion>> GetAllAsync(params Guid[] ids) =>
        ids.Length > 0
            ? await _applicationDbContext.Champions.Where(c => ids.Contains(c.Id)).AsNoTracking().ToListAsync()
            : await _applicationDbContext.Champions.AsNoTracking().ToListAsync();
}