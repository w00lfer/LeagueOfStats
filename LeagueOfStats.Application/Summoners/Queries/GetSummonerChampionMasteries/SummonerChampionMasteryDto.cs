namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMasteries;

public record SummonerChampionMasteryDto(
    int ChampionId,
    string ChampionName,
    int ChampionLevel,
    string ChampionTitle,
    string ChampionDescription,
    string ChampionImageFullFileName,
    string ChampionImageSpriteFileName,
    int ChampionImageHeight,
    int ChampionImageWidth,
    int ChampionPoints,
    bool ChestGranted);