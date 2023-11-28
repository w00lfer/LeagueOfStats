using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Summoners;

// Aggregate member of Summoner
public class SummonerChampionMastery : Entity
{
    public SummonerChampionMastery(int championId, int championLevel, int championPoints, long championPointsSinceLastLevel, long championPointsUntilNextLevel, bool chestGranted, long lastPlayTime, int tokensEarned)
    {
        ChampionId = championId;
        ChampionLevel = championLevel;
        ChampionPoints = championPoints;
        ChampionPointsSinceLastLevel = championPointsSinceLastLevel;
        ChampionPointsUntilNextLevel = championPointsUntilNextLevel;
        ChestGranted = chestGranted;
        LastPlayTime = lastPlayTime;
        TokensEarned = tokensEarned;
    }
    
    public int Id { get; }
    
    public int ChampionId { get; set; }
    
    public int ChampionLevel { get; set; }
    
    public int ChampionPoints { get; set; }
    
    public long ChampionPointsSinceLastLevel { get; set; }
    
    public long ChampionPointsUntilNextLevel { get; set; }
    
    public bool ChestGranted { get; set; }
    
    public long LastPlayTime { get; set; }
    
    public int TokensEarned { get; set; }
}