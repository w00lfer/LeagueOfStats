using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Summoners;

// Aggregate member of Summoner
public class SummonerChampionMastery : Entity
{
    public SummonerChampionMastery(int riotChampionId, int championLevel, int championPoints, long championPointsSinceLastLevel, long championPointsUntilNextLevel, bool chestGranted, long lastPlayTime, int tokensEarned)
        : base(Guid.NewGuid())
    {
        RiotChampionId = riotChampionId;
        ChampionLevel = championLevel;
        ChampionPoints = championPoints;
        ChampionPointsSinceLastLevel = championPointsSinceLastLevel;
        ChampionPointsUntilNextLevel = championPointsUntilNextLevel;
        ChestGranted = chestGranted;
        LastPlayTime = lastPlayTime;
        TokensEarned = tokensEarned;
    }
    
    public int RiotChampionId { get; }
    
    public int ChampionLevel { get; private set; }
    
    public int ChampionPoints { get; private set; }
    
    public long ChampionPointsSinceLastLevel { get; private set; }
    
    public long ChampionPointsUntilNextLevel { get; private set; }
    
    public bool ChestGranted { get; private set; }
    
    public long LastPlayTime { get; private set; }
    
    public int TokensEarned { get; private set; }

    internal void Update(int championLevel, int championPoints, long championPointsSinceLastLevel, long championPointsUntilNextLevel, bool chestGranted, long lastPlayTime, int tokensEarned)
    {
        ChampionLevel = championLevel;
        ChampionPoints = championPoints;
        ChampionPointsSinceLastLevel = championPointsSinceLastLevel;
        ChampionPointsUntilNextLevel = championPointsUntilNextLevel;
        ChestGranted = chestGranted;
        LastPlayTime = lastPlayTime;
        TokensEarned = tokensEarned;
    }
}