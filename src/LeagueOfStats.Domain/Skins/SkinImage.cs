using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Skins;

public class SkinImage : ValueObject
{
    private SkinImage(
        string splashUrl,
        string uncenteredSplashUrl,
        string tileUrl)
    {
        SplashUrl = splashUrl;
        UncenteredSplashUrl = uncenteredSplashUrl;
        TileUrl = tileUrl;
    }

    public static SkinImage Create(
        string splashUrl,
        string uncenteredSplashUrl,
        string tileUrl) =>
        new(splashUrl, uncenteredSplashUrl, tileUrl);
    
    public string SplashUrl { get; }
    
    public string UncenteredSplashUrl { get; }
    
    
    public string TileUrl { get; }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return SplashUrl;
        yield return UncenteredSplashUrl;
        yield return TileUrl;
    }
}