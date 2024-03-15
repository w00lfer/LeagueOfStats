using LeagueOfStats.Domain.Matches.Teams.Enums;

namespace LeagueOfStats.Domain.Matches.Teams.Dtos;

public record AddTeamDto(
    AddObjectivesDto AddObjectivesDto,
    IEnumerable<AddBanDto> AddBanDtos,
    Side Side,
    bool Win);