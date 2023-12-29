using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Domain.Matches;

public class MatchDomainService : IMatchDomainService
{
    private readonly IMatchRepository _matchRepository;

    public MatchDomainService(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<Result<Match>> GetByIdAsync(Guid id)
    {
        Match? match = await _matchRepository.GetByIdAsync(id);
        
        return match is not null
            ? match
            : new EntityNotFoundError($"Match with Id={id} does not exist.");
    }

    public async Task<Result<IEnumerable<Match>>> AddMatchesAsync(IEnumerable<AddMatchDto> addMatchDtos)
    {
        var alreadyExistingMatches = (await _matchRepository.GetAllByRiotMatchIdsAsync(addMatchDtos.Select(m => m.RiotMatchId))).ToList();
        List<string> alreadyExistingMatchRiotMatchIds = alreadyExistingMatches.Select(m => m.RiotMatchId).ToList();

        var addMatchDtosToBeAdded = addMatchDtos.Where(addMatchDto => alreadyExistingMatchRiotMatchIds.Contains(addMatchDto.RiotMatchId) is false);

        return await ValidateNumberOfSummonersInMatches(addMatchDtosToBeAdded)
            .Bind(async () =>
            {
                var matchesToAdd = addMatchDtosToBeAdded.Select(addMatchDto => new Match(addMatchDto.RiotMatchId, addMatchDto.Summoners.Select(s => s.Id), addMatchDto.GameEndedTimestamp));


                await _matchRepository.AddRangeAsync(matchesToAdd);

                var matches = alreadyExistingMatches.Concat(matchesToAdd).OrderByDescending(m => m.GameEndTimestamp).AsEnumerable();

                return Result.Success(matches);
            });
    }

    private Result ValidateNumberOfSummonersInMatches(IEnumerable<AddMatchDto> addMatchDtos)
    {
        var invalidAddMatchDtos = addMatchDtos.Where(m => IsNumberOfSummonerInMatchValid(m) is false).ToList();

        return invalidAddMatchDtos.Count == 0
            ? Result.Success()
            : new DomainError($"{invalidAddMatchDtos.Select(m => $"Could not create Match with GameType=GameType and number of Summoners={m.Summoners.Count()}")}");
    }

    private bool IsNumberOfSummonerInMatchValid(AddMatchDto addMatchDto) =>
        true;
}