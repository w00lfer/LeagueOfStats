using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Summoners.Dtos;
using NodaTime;

namespace LeagueOfStats.Domain.Summoners;

public class Summoner : AggregateRoot
{
    private readonly List<SummonerChampionMastery> _summonerChampionMasteries = new();
    
    internal Summoner(
        string summonerId,
        string accountId,
        string name,
        int profileIconId,
        string puuid,
        long summonerLevel,
        string gameName,
        string tagLine,
        Region region,
        IEnumerable<UpdateChampionMasteryDto> updateChampionMasteryDtos,
        Instant lastUpdated)
        : base(Guid.NewGuid())
    {
        SummonerId = summonerId;
        AccountId = accountId;
        SummonerName = SummonerName.Create(gameName, tagLine);
        Name = name;
        ProfileIconId = profileIconId;
        Puuid = puuid;
        SummonerLevel = summonerLevel;
        Region = region;
        LastUpdated = lastUpdated;

        _summonerChampionMasteries.AddRange(updateChampionMasteryDtos
            .Select(championMasteryForAdd =>
                new SummonerChampionMastery(
                    this,
                    championMasteryForAdd.Champion,
                    championMasteryForAdd.ChampionLevel,
                    championMasteryForAdd.ChampionPoints,
                    championMasteryForAdd.ChampionPointsSinceLastLevel,
                    championMasteryForAdd.ChampionPointsUntilNextLevel,
                    championMasteryForAdd.ChestGranted,
                    championMasteryForAdd.LastPlayTime,
                    championMasteryForAdd.TokensEarned)));
    }

    protected Summoner()
        : base(Guid.Empty)
    {
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

    public List<SummonerChampionMastery> SummonerChampionMasteries => _summonerChampionMasteries.ToList();

    internal void Update(
        int profileIconId,
        long summonerLevel,
        IEnumerable<UpdateChampionMasteryDto> updateChampionMasteryDtos,
        Instant lastUpdated)
    {
        ProfileIconId = profileIconId;
        SummonerLevel = summonerLevel;
        LastUpdated = lastUpdated;
        
        UpdateSummonerChampionMasteries(updateChampionMasteryDtos);
    }

    private void UpdateSummonerChampionMasteries(IEnumerable<UpdateChampionMasteryDto> updateChampionMasteryDtos)
    {
        var existingMasteryForChampionIds = _summonerChampionMasteries
            .Select(c => c.ChampionId)
            .ToList();
        
        var updateChampionMasteryDtoToBeUpdated = updateChampionMasteryDtos
                .Where(c => existingMasteryForChampionIds.Contains(c.Champion.Id));
        
        foreach (var updateChampionMasteryDto in updateChampionMasteryDtoToBeUpdated)
        {
            var summonerChampionMasteryToUpdate =
                _summonerChampionMasteries.Single(c =>
                    c.ChampionId == updateChampionMasteryDto.Champion.Id);
            
            summonerChampionMasteryToUpdate.Update(
                updateChampionMasteryDto.ChampionLevel,
                updateChampionMasteryDto.ChampionPoints,
                updateChampionMasteryDto.ChampionPointsSinceLastLevel,
                updateChampionMasteryDto.ChampionPointsUntilNextLevel,
                updateChampionMasteryDto.ChestGranted,
                updateChampionMasteryDto.LastPlayTime,
                updateChampionMasteryDto.TokensEarned);
        }

        var championMasteriesForAdd = updateChampionMasteryDtos
            .Where(c => existingMasteryForChampionIds.Contains(c.Champion.Id) is false);
        
        foreach (var championMasteryForAdd in championMasteriesForAdd)
        {
            _summonerChampionMasteries.Add(
                new SummonerChampionMastery(
                    this,
                    championMasteryForAdd.Champion,
                    championMasteryForAdd.ChampionLevel,
                    championMasteryForAdd.ChampionPoints,
                    championMasteryForAdd.ChampionPointsSinceLastLevel,
                    championMasteryForAdd.ChampionPointsUntilNextLevel,
                    championMasteryForAdd.ChestGranted,
                    championMasteryForAdd.LastPlayTime,
                    championMasteryForAdd.TokensEarned));
        }
    }
}