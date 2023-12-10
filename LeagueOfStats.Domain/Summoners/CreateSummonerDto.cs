using LeagueOfStats.Domain.Common.Enums;

namespace LeagueOfStats.Domain.Summoners;

public record CreateSummonerDto(
    string SummonerId,
    string AccountId,
    string Name,
    int ProfileIconId,
    string Puuid,
    long SummonerLevel,
    string GameName,
    string TagLine,
    Region Region);