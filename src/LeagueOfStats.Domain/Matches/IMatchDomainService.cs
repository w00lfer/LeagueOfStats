using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Domain.Matches;

public interface IMatchDomainService
{
    public Task<Result<Match>> GetByIdAsync(Guid id);
    
    public Task<Result<IEnumerable<Match>>> AddMatchesAsync(IEnumerable<AddMatchDto> addMatchDtos);
}