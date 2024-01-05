namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerLiveGame;

public record LiveGameTeamParticipantDto(
    Guid ChampionId,
    string ChampionName,
    string ChampionImageFullFileName,
    string SummonerName,
    bool IsBot,
    int ProfileIconId,
    long Spell1Id,
    long Spell2Id);