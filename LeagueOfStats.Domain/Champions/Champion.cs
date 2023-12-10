using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Champions;

public class Champion : AggregateRoot<ChampionId>
{
    public Champion(int id, string name, string title, string description, ChampionImage championImage)
        : base(new ChampionId(id))
    {
        Name = name;
        Title = title;
        Description = description;
        ChampionImage = championImage;
    }
    
    public string Name { get; }
        
    public string Title { get; }
        
    public string Description { get; }
        
    public ChampionImage ChampionImage { get; } 
}