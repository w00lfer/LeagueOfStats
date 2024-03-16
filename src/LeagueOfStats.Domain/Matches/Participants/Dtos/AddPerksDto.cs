namespace LeagueOfStats.Domain.Matches.Participants.Dtos;

public record AddPerksDto(
    AddPerkStatsDto AddPerkStatsDto,
    IEnumerable<AddPerkStyleDto> AddPerkStyleDtos);