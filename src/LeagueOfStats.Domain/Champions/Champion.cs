using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Champions;

public class Champion : AggregateRoot
{
    public Champion(
        int riotChampionId,
        string name,
        string title,
        string description,
        ChampionImage championImage)
        : base(Guid.NewGuid())
    {
        RiotChampionId = riotChampionId;
        Name = name;
        Title = title;
        Description = description;
        ChampionImage = championImage;
    }

    protected Champion()
        : base(Guid.NewGuid())
    {
    }

    public int RiotChampionId { get; }
    
    public string Name { get; }
        
    public string Title { get; }
        
    public string Description { get; }
        
    public ChampionImage ChampionImage { get; } 
}