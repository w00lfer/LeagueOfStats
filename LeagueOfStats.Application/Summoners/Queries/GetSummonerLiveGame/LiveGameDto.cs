using LeagueOfStats.Domain.Matches.Enums;
using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerLiveGame;

public record LiveGameDto(
    IEnumerable<LiveGameBannedChampionDto> BannedChampion,
    IEnumerable<LiveGameTeamDto> Teams,
    Duration Duration,
    GameMode GameMode,
    Instant GameStartTime,
    GameType GameType,
    Map Map);