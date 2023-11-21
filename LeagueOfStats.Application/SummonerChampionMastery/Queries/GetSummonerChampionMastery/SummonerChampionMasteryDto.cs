namespace LeagueOfStats.Application.SummonerChampionMastery.Queries.GetSummonerChampionMastery
{
    public record SummonerChampionMasteryDto(
        int ChampionId,
        string ChampionName,
        string ChampionTitle,
        string ChampionDescription,
        string ChampionImageFullFileName,
        string ChampionImageSpriteFileName,
        int ChampionImageHeight,
        int ChampionImageWidth,
        int ChampionPoints,
        bool ChestGranted);
}