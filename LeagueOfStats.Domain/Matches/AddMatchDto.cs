using LeagueOfStats.Domain.Summoners;
using NodaTime;

namespace LeagueOfStats.Domain.Matches;

public record AddMatchDto(
    string RiotMatchId,
    IEnumerable<Summoner> Summoners,
    Instant GameEndedTimestamp);