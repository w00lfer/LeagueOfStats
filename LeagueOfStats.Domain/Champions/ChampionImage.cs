using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Champions;

public class ChampionImage : ValueObject
{
    private ChampionImage(
        string fullFileName,
        string spriteFileName,
        int width,
        int height)
    {
        FullFileName = fullFileName;
        SpriteFileName = spriteFileName;
        Width = width;
        Height = height;
    }

    public static ChampionImage Create(
        string fullFileName,
        string spriteFileName,
        int width,
        int height) =>
        new(fullFileName, spriteFileName, width, height);
    
    public string FullFileName { get; }
        
    public string SpriteFileName { get; }
        
    public int Width { get; }
        
    public int Height { get; }
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return FullFileName;
        yield return SpriteFileName;
        yield return Width;
        yield return Height;
    }
}