using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using LeagueOfStats.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.Summoners;

public class SummonerRepository : AsyncRepositoryBase<Summoner>, ISummonerRepository
{
    public SummonerRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext)
    {
    }

    public Task<Summoner?> GetByIdWithAllIncludesAsync(Guid id) =>
        _applicationDbContext.Summoners
            .Include(s => s.SummonerChampionMasteries)
            .SingleOrDefaultAsync(s => s.Id == id);

    public async Task<Summoner?> GetByPuuidAsync(string puuid) =>
        await _applicationDbContext.Summoners.SingleOrDefaultAsync(s => s.Puuid == puuid);

    public async Task<IEnumerable<Summoner>> GetByPuuidsAsync(IEnumerable<string> puuids) =>
        puuids.Any()
            ? await _applicationDbContext.Summoners.Where(s => puuids.Contains(s.Puuid)).ToListAsync()
            : await _applicationDbContext.Summoners.ToListAsync();
}