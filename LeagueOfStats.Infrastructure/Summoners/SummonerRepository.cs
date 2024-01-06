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

    public Task<Summoner?> GetByPuuidAsync(string puuid) =>
        _applicationDbContext.Summoners.SingleOrDefaultAsync(s => s.Puuid == puuid);

    public async Task<IEnumerable<Summoner>> GetAllAsync(params Guid[] ids) =>
        (ids.Length > 0
            ? await _applicationDbContext.Summoners.Where(s => ids.Contains(s.Id)).ToListAsync()
            : await _applicationDbContext.Summoners.ToListAsync());

    public async Task AddAsync(Summoner entity)
    {
        _applicationDbContext.Summoners.AddAsync(entity);

        await _applicationDbContext.SaveChangesAsync();
    }

    public Task AddRangeAsync(IEnumerable<Summoner> entities)
    {
        throw new NotImplementedException();
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