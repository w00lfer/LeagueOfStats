using LeagueOfStats.Domain.Summoners;
using NodaTime;

namespace LeagueOfStats.Application.Summoners;

public record SummonerDto(
    Guid Id,
    string AccountId,
    string SummonerId,
    string Name,
    int ProfileIconId,
    string Puuid,
    long SummonerLevel,
    SummonerName SummonerName,
    Instant LastUpdated);