using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Champions;

public class Champion : Entity, IAggregateRoot
{
    public Champion(int id, string name, string title, string description, ChampionImage championImage)
    {
        Id = id;
        Name = name;
        Title = title;
        Description = description;
        ChampionImage = championImage;
    }

    public int Id { get; }
    public string Name { get; }
        
    public string Title { get; }
        
    public string Description { get; }
        
    public ChampionImage ChampionImage { get; } 
}