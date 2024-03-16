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
    
    public async Task<IEnumerable<Skin>> GetByRiotIdsAsync(int[] riotIds) =>
        riotIds.Length > 0
            ? await _applicationDbContext.Skins.Where(c => riotIds.Contains(c.RiotSkinId)).AsNoTracking().ToListAsync()
            : await _applicationDbContext.Skins.AsNoTracking().ToListAsync();
}