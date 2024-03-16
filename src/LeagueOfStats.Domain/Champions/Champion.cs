using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Champions;

public class Champion : AggregateRoot
{
    public Champion(
        int riotChampionId,
        string name,
        string title,
        string description,
        string splashUrl,
        string uncenteredSplashUrl,
        string iconUrl,
        string tileUrl)
        : base(Guid.NewGuid())
    {
        RiotChampionId = riotChampionId;
        Name = name;
        Title = title;
        Description = description;
        ChampionImage = ChampionImage.Create(
            splashUrl,
            uncenteredSplashUrl,
            iconUrl,
            tileUrl);
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