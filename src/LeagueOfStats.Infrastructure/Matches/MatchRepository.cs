using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using LeagueOfStats.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.Matches;

public class MatchRepository : AsyncRepositoryBase<Match>, IMatchRepository
{
    public MatchRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext)
    {
    }

    public Task<Match?> GetByIdWithAllIncludesAsync(Guid id) =>
        _applicationDbContext.Matches
            .Include(m => m.Teams).ThenInclude(t => t.Bans)
            .Include(m => m.Participants).ThenInclude(p => p.Perks.StatPerks)
            .Include(m => m.Participants).ThenInclude(p => p.Perks.Styles).ThenInclude(s => s.Selections)
            .SingleOrDefaultAsync(m => m.Id == id);
    
    public async Task<IEnumerable<Match>> GetAllByRiotMatchIdsWithAllIncludesAsync(IEnumerable<string> riotMatchIds)
    {
        IQueryable<Match> matchesQueryable = 
            riotMatchIds.Any() 
                ? _applicationDbContext.Matches.Where(m => riotMatchIds.Contains(m.RiotMatchId))
                : _applicationDbContext.Matches;
            
            return await matchesQueryable
                .Include(m => m.Teams).ThenInclude(t => t.Bans)
                .Include(m => m.Participants).ThenInclude(p => p.Perks.StatPerks)
                .Include(m => m.Participants).ThenInclude(p => p.Perks.Styles).ThenInclude(s => s.Selections)
                .ToListAsync();
    }
}