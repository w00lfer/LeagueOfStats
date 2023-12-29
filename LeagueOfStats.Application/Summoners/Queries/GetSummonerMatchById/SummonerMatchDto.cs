using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchById;

public record SummonerMatchDto(
    Guid Id,
    string RiotMatchId,
    IEnumerable<Guid> SummonerIds,
    Instant GameEndedTimestamp);