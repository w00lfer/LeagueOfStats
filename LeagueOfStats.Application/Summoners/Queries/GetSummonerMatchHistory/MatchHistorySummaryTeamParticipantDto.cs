namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public record MatchHistorySummaryTeamParticipantDto(
    Guid ChampionId,
    string ChampionName,
    string ChampionImageFullFileName);