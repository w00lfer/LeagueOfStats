using NodaTime;

namespace LeagueOfStats.Domain.Summoners;

// Aggregate root
public class Summoner
{
    public Summoner(int id, string accountId, string name, int profileIconId, string puuid, long revisionDate, long summonerLevel, LocalDate lastUpdated, IEnumerable<SummonerChampionMastery> summonerChampionMasteries)
    {
        Id = id;
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
}