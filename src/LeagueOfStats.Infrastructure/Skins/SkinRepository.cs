using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using LeagueOfStats.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.Skins;

public class SkinRepository : AsyncRepositoryBase<Skin>, ISkinRepository
{
    public SkinRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext)
    {
    } 
    
    public async Task<IEnumerable<Skin>> GetByRiotSkinIdsAsync(IEnumerable<int> riotSkinIds) =>
        riotSkinIds.Any()
            ? await _applicationDbContext.Skins.Where(c => riotSkinIds.Contains(c.RiotSkinId)).AsNoTracking().ToListAsync()
            : await _applicationDbContext.Skins.AsNoTracking().ToListAsync();
}