using LeagueOfStats.Domain.Matches.Teams.Enums;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerLiveGame;

public record LiveGameBannedChampionDto(
    Guid ChampionId,
    string ChampionName,
    string ChampionImageFullFileName,
    int PickTurn,
    Side Side);