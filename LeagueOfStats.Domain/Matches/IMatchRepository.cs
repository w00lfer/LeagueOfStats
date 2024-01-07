using LeagueOfStats.Domain.Common.Repositories;

namespace LeagueOfStats.Domain.Matches;

public interface IMatchRepository : IAsyncRepository<Match>
{
    Task<IEnumerable<Match>> GetAllByRiotMatchIdsIncludingRelatedEntitiesAsync(IEnumerable<string> riotMatchIds);
}