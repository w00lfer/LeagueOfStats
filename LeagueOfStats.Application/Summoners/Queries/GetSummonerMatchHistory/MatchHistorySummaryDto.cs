using LeagueOfStats.Domain.Matches;
using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public record MatchHistorySummaryDto(
    Guid Id,
    MatchHistorySummarySummonerDto Summoner,
    IEnumerable<MatchHistorySummaryTeamDto> Teams,
    string GameVersion,
    Duration GameDuration,
    Instant GameStartTimeStamp,
    Instant GameEndTimeStamp,
    GameMode GameMode,
    GameType GameType,
    Map Map);