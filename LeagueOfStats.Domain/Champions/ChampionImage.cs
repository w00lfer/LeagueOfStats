using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Champions;

public class ChampionImage : Entity<ChampionImageId>
{
    public ChampionImage(string fullFileName, string spriteFileName, int width, int height)
        : base(new ChampionImageId(Guid.NewGuid()))
    {
        FullFileName = fullFileName;
        SpriteFileName = spriteFileName;
        Width = width;
        Height = height;
    }
    
    public string FullFileName { get; }
        
    public string SpriteFileName { get; }
        
    public int Width { get; }
        
    public int Height { get; }
}