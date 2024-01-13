using LeagueOfStats.Application.Common.Enums;
using LeagueOfStats.Domain.Common.Enums;
using NodaTime;

namespace LeagueOfStats.Application.RiotClient;

public record GetSummonerMatchHistoryDto(
    Region Region,
    string Puuid,
    int Count,
    Instant GameEndedAt,
    MatchHistoryQueueFilter MatchHistoryQueueFilter);