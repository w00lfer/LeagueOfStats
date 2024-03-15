using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Champions;

public class ChampionImage : ValueObject
{
    private ChampionImage(
        string splashUrl,
        string uncenteredSplashUrl,
        string iconUrl,
        string tileUrl)
    {
        SplashUrl = splashUrl;
        UncenteredSplashUrl = uncenteredSplashUrl;
        IconUrl = iconUrl;
        TileUrl = tileUrl;
    }

    public static ChampionImage Create(
        string splashUrl,
        string uncenteredSplashUrl,
        string iconUrl,
        string tileUrl) =>
        new(splashUrl, uncenteredSplashUrl, iconUrl, tileUrl);
    
    public string SplashUrl { get; }
    
    public string UncenteredSplashUrl { get; }
        
    public string IconUrl { get; }
    
    public string TileUrl { get; }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return SplashUrl;
        yield return UncenteredSplashUrl;
        yield return IconUrl;
        yield return TileUrl;
    }
}