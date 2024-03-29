using LeagueOfStats.Domain.Common.Repositories;

namespace LeagueOfStats.Domain.Matches;

public interface IMatchRepository : IAsyncRepository<Match>
{
    Task<Match?> GetByIdWithAllIncludesAsync(Guid id);
    
    Task<IEnumerable<Match>> GetAllByRiotMatchIdsWithAllIncludesAsync(IEnumerable<string> riotMatchIds);
}