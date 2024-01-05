namespace LeagueOfStats.Domain.Summoners.Dtos;

public record UpdateChampionMasteryDto(
    int RiotChampionId,
    int ChampionLevel,
    int ChampionPoints,
    long ChampionPointsSinceLastLevel,
    long ChampionPointsUntilNextLevel,
    bool ChestGranted,
    long LastPlayTime,
    int TokensEarned);