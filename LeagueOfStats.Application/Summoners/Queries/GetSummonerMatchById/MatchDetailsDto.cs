using LeagueOfStats.Domain.Matches.Enums;
using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchById;

public record MatchDetailsDto(
    Guid Id,
    IEnumerable<MatchDetailsTeamDto> Teams,
    string GameVersion,
    Duration GameDuration,
    Instant GameStartTimeStamp,
    Instant GameEndTimeStamp,
    GameMode GameMode,
    GameType GameType,
    Map Map);