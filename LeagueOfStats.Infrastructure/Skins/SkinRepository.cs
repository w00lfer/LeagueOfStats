using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.Skins;

public class SkinRepository : ISkinRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SkinRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public Task<Skin?> GetByIdAsync(Guid id) =>
        _applicationDbContext.Skins.SingleOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Skin>> GetAllAsync(params Guid[] ids) =>
        ids.Length > 0
            ? await _applicationDbContext.Skins.Where(c => ids.Contains(c.Id)).AsNoTracking().ToListAsync()
            : await _applicationDbContext.Skins.AsNoTracking().ToListAsync();
}