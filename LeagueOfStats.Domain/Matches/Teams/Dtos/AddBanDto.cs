using LeagueOfStats.Domain.Champions;

namespace LeagueOfStats.Domain.Matches.Teams.Dtos;

public record AddBanDto(
    Champion Champion,
    int PickTurn);