using NodaTime;

namespace LeagueOfStats.Domain.Matches;

public record AddMatchDto(
    string RiotMatchId,
    Instant GameEndedTimestamp);