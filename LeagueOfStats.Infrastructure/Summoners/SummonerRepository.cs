using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.Summoners;

public class SummonerRepository : ISummonerRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SummonerRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public Task<Summoner?> GetByIdAsync(Guid id) =>
        _applicationDbContext.Summoners.SingleOrDefaultAsync(s => s.Id == id);

    public async Task<Summoner?> GetByPuuidAsync(string puuid) =>
        await _applicationDbContext.Summoners.SingleOrDefaultAsync(s => s.Puuid == puuid);

    public async Task<IEnumerable<Summoner>> GetByPuuidsAsync(IEnumerable<string> puuids) =>
        puuids.Any()
            ? await _applicationDbContext.Summoners.Where(s => puuids.Contains(s.Puuid)).ToListAsync()
            : await _applicationDbContext.Summoners.ToListAsync();

    public async Task<IEnumerable<Summoner>> GetAllAsync(params Guid[] ids) =>
        ids.Length > 0
            ? await _applicationDbContext.Summoners.Where(s => ids.Contains(s.Id)).ToListAsync()
            : await _applicationDbContext.Summoners.ToListAsync();

    public async Task AddAsync(Summoner entity)
    {
        await _applicationDbContext.Summoners.AddAsync(entity);

        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Summoner> entities)
    {
        await _applicationDbContext.Summoners.AddRangeAsync(entities);

        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Summoner entity)
    {
        _applicationDbContext.Summoners.Remove(entity);

        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Summoner entity)
    {
        await _applicationDbContext.SaveChangesAsync();
    }
}