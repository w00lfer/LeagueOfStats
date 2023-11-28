namespace LeagueOfStats.Application.SummonerChampionMastery.Queries.GetSummonersByName;

public record SummonerDto(
    string AccountId,
    string Id,
    string Name,
    int ProfileIconId,
    string Puuid,
    long RevisionDate,
    long SummonerLevel,
    string RiotId);