namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public record MatchHistorySummarySummonerDto(
    Guid ChampionId,
    string ChampionName,
    string ChampionImageFullFileName,
    int ChampLevel,
    int Kills,
    int Deaths,
    int Assists,
    double Kda,
    int TotalMinionsKilled,
    double TotalMinionsKilledPerMinute,
    int Item0,
    int Item1,
    int Item2,
    int Item3,
    int Item4,
    int Item5,
    int Item6);