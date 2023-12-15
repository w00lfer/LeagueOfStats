namespace LeagueOfStats.Domain.Summoners;

public record UpdateDetailsSummonerDto(
    int ProfileIconId,
    long SummonerLevel,
    IEnumerable<UpdateChampionMasteryDto> UpdateChampionMasteryDtos);