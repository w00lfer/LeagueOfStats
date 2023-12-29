using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public record SummonerMatchHistorySummaryDto(
    Guid Id,
    string RiotMatchId,
    IEnumerable<Guid> SummonerIds,
    Instant GameEndedTimestamp);