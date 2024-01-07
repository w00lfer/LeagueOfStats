using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.Matches;

public class MatchRepository : IMatchRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public MatchRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public Task<Match?> GetByIdAsync(Guid id) =>
        _applicationDbContext.Matches.SingleOrDefaultAsync(m => m.Id == id);

    public Task<IEnumerable<Match>> GetAllAsync(params Guid[] ids)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Match entity)
    {
        throw new NotImplementedException();
    }

    public async Task AddRangeAsync(IEnumerable<Match> entities)
    {
        await _applicationDbContext.Matches.AddRangeAsync(entities);

        await _applicationDbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(Match entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Match entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Match>> GetAllByRiotMatchIdsAsync(IEnumerable<string> riotMatchIds) =>
        await _applicationDbContext.Matches.Where(m => riotMatchIds.Contains(m.RiotMatchId)).ToListAsync();
}