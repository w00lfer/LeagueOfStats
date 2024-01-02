using LeagueOfStats.Domain.Matches;

namespace LeagueOfStats.Infrastructure.Matches;

public class MatchRepository : IMatchRepository
{
    private readonly List<Match> _matches = new();
    
    public Task<Match?> GetByIdAsync(Guid id) => 
        Task.FromResult(_matches.FirstOrDefault(m => m.Id == id));

    public Task<IEnumerable<Match>> GetAllAsync(params Guid[] ids)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Match entity)
    {
        throw new NotImplementedException();
    }

    public Task AddRangeAsync(IEnumerable<Match> entities)
    {
        _matches.AddRange(entities);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Match entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Match entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Match>> GetAllByRiotMatchIdsAsync(IEnumerable<string> riotMatchIds) => 
        Task.FromResult(_matches.Where(m => riotMatchIds.Contains(m.RiotMatchId)));
}