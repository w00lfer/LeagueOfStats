namespace LeagueOfStats.Domain.Matches;

public class MatchDomainService : IMatchDomainService
{
    private readonly IMatchRepository _matchRepository;

    public MatchDomainService(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<IEnumerable<Match>> AddMatches(IEnumerable<AddMatchDto> addMatchDtos)
    {
        var alreadyExistingMatches = (await _matchRepository.GetAllByRiotMatchIdsAsync(addMatchDtos.Select(m => m.RiotMatchId))).ToList();
        List<string> arleadyExistingMatchRiotMatchIds = alreadyExistingMatches.Select(m => m.RiotMatchId).ToList();

        var addMatchDtosToBeAdded = addMatchDtos.Where(addMatchDto => arleadyExistingMatchRiotMatchIds.Contains(addMatchDto.RiotMatchId) is false);

        var matchesToAdd = addMatchDtosToBeAdded.Select(addMatchDto => new Match(addMatchDto.RiotMatchId, addMatchDto.GameEndedTimestamp));
        await _matchRepository.AddRangeAsync(matchesToAdd);

        return alreadyExistingMatches.Concat(matchesToAdd).OrderByDescending(m => m.GameEndTimestamp);
    }
}