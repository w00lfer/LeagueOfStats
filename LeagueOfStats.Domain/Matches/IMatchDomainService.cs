namespace LeagueOfStats.Domain.Matches;

public interface IMatchDomainService
{
    public Task<IEnumerable<Match>> AddMatches(IEnumerable<AddMatchDto> addMatchDtos);
}