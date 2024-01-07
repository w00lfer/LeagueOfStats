using LeagueOfStats.Domain.Champions;

namespace LeagueOfStats.Domain.Summoners.Dtos;

public record UpdateChampionMasteryDto(
    Champion Champion,
    int ChampionLevel,
    int ChampionPoints,
    long ChampionPointsSinceLastLevel,
    long ChampionPointsUntilNextLevel,
    bool ChestGranted,
    long LastPlayTime,
    int TokensEarned);