using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Summoners;

// Aggregate member of Summoner
public class SummonerChampionMastery : Entity<SummonerChampionMasteryId>
{
    public SummonerChampionMastery(ChampionId championId, int championLevel, int championPoints, long championPointsSinceLastLevel, long championPointsUntilNextLevel, bool chestGranted, long lastPlayTime, int tokensEarned)
        : base (new SummonerChampionMasteryId(Guid.NewGuid()))
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
    
    public ChampionId ChampionId { get; }
    
    public int ChampionLevel { get; }
    
    public int ChampionPoints { get; }
    
    public long ChampionPointsSinceLastLevel { get; }
    
    public long ChampionPointsUntilNextLevel { get; }
    
    public bool ChestGranted { get; }
    
    public long LastPlayTime { get; }
    
    public int TokensEarned { get; }
}