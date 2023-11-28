using LeagueOfStats.Domain.Common.Entities;
using NodaTime;

namespace LeagueOfStats.Domain.Summoners;

public class Summoner : Entity, IAggregateRoot
{
    public Summoner(string accountId, string name, int profileIconId, string puuid, long summonerLevel, LocalDate lastUpdated, IEnumerable<SummonerChampionMastery> summonerChampionMasteries)
    {
        AccountId = accountId;
        Name = name;
        ProfileIconId = profileIconId;
        Puuid = puuid;
        SummonerLevel = summonerLevel;
        LastUpdated = lastUpdated;
        SummonerChampionMasteries = summonerChampionMasteries;
    }

    public int Id { get; }
    
    public string AccountId { get; }
    
    public string Name { get; }
    
    public string Puuid { get; }
    
    public int ProfileIconId { get; }
    
    public long SummonerLevel { get; }
    
    public LocalDate LastUpdated { get; }

    public IEnumerable<SummonerChampionMastery> SummonerChampionMasteries { get; }

    public void UpdateSummonerChampionMasteries(IEnumerable<SummonerChampionMastery> SummonerChampionMasteries)
    {
        
    }
}