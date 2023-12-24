using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public record MatchHistoryDto(
    string RiotMatchId,
    Instant GameEndedTimestamp);