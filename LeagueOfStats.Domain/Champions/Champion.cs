using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Champions;

public class Champion : AggregateRoot
{
    public Champion(
        int id,
        string name,
        string title,
        string description,
        ChampionImage championImage)
        : base(Guid.NewGuid())
    {
        RiotChampionId = id;
        Name = name;
        Title = title;
        Description = description;
        ChampionImage = championImage;
    }

    private Champion()
        : base(Guid.NewGuid())
    {
    }

    public int RiotChampionId { get; }
    
    public string Name { get; }
        
    public string Title { get; }
        
    public string Description { get; }
        
    public ChampionImage ChampionImage { get; } 
}