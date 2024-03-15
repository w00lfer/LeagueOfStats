namespace LeagueOfStats.Domain.Summoners.Dtos;

public record UpdateDetailsSummonerDto(
    int ProfileIconId,
    long SummonerLevel,
    IEnumerable<UpdateChampionMasteryDto> UpdateChampionMasteryDtos);