namespace LeagueOfStats.Domain.Matches.Participants.Dtos;

public record AddPerkStyleDto(
    IEnumerable<AddPerkStyleSelectionDto> AddPerkStyleSelectionDtos,
    string Description,
    int Style);