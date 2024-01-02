namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public record MatchHistorySummaryTeamParticipantDto(
    Guid ChampionId,
    Guid? SummonerId,
    string SummonerName,
    string ChampionName,
    string ChampionImageFullFileName);