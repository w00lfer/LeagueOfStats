using LeagueOfStats.Domain.Common;
using LeagueOfStats.Domain.Common.Constants;
using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Common.Enums;
using NodaTime;

namespace LeagueOfStats.Domain.Summoners;

public class Summoner : AggregateRoot
{
    public Summoner(string summonerId, string accountId, string name, int profileIconId, string puuid, long summonerLevel, SummonerName summonerName, Region region)
        : base (Guid.NewGuid())
    {
        SummonerId = summonerId;
        AccountId = accountId;
        SummonerName = summonerName;
        Name = name;
        ProfileIconId = profileIconId;
        Puuid = puuid;
        SummonerLevel = summonerLevel;
        Region = region;
        LastUpdated = Clock.GetCurrentInstant();
    }
    
    
    public string SummonerId { get; }
    
    public string AccountId { get; }

    // TODO Think about how to update summoner name and how to know it's changed
    public SummonerName SummonerName { get; private set; }
    
    public string Name { get; }
    
    public string Puuid { get; }
    
    public int ProfileIconId { get; private set; }
    
    public long SummonerLevel { get; private set; }
    
    public Region Region { get; }
    
    public Instant LastUpdated { get; private set; }

    public bool CanBeUpdated =>
        Clock.GetCurrentInstant().Minus(LastUpdated).TotalMinutes >= UpdateLockoutConstants.GetSummonerUpdateLockoutInMinutes;

    public void Update(int profileIconId, long summonerLevel)
    {
        // TODO Think how to split this into dev, prod and tests (read it as options?)
        if (CanBeUpdated is false)
        {
            return;
        }
        
        ProfileIconId = profileIconId;
        SummonerLevel = summonerLevel;
        LastUpdated = Clock.GetCurrentInstant();
    }
}