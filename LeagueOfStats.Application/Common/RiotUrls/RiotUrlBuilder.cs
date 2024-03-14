namespace LeagueOfStats.Application.Common.RiotUrls;

public static class RiotUrlBuilder
{
    private const string CommunityDragonUrl = "raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/";

    private const string SplashPostfixUrl = "champion-splashes/";
    private const string UncenteredSplashPostfixUrl = "champion-splashes/uncentered/";
    private const string ChampionIconPostfixUrl = "champion-icons/";
    private const string ChampionTilePostfixUrl = "champion-tiles/";
    
    
    public static string GetChampionSplashByRiotChampionId(int championRiotId)
    {
        var identifierPostfix = $"{championRiotId}/{championRiotId}000.jpg";
        
        var combinedPath = string.Concat(CommunityDragonUrl, SplashPostfixUrl, identifierPostfix);

        return combinedPath;
    }
    
    public static string GetChampionSkinSplashByRiotChampionId(int skinRiotId)
    {
        string skinRiotIdAsString = skinRiotId.ToString();
        string championRiotId = skinRiotIdAsString[..^3];
        
        var identifierPostfix = $"{championRiotId}/{skinRiotId}.jpg";
        
        var combinedPath = string.Concat(CommunityDragonUrl, SplashPostfixUrl, identifierPostfix);

        return combinedPath;
    }
    
    public static string GetChampionUncenteredSplashByRiotChampionId(int championRiotId)
    {
        var identifierPostfix = $"{championRiotId}/{championRiotId}000.jpg";
        
        var combinedPath = string.Concat(CommunityDragonUrl, UncenteredSplashPostfixUrl, identifierPostfix);

        return combinedPath;
    }
    
    public static string GetChampionSkinUncenteredSplashByRiotChampionId(int skinRiotId)
    {
        string skinRiotIdAsString = skinRiotId.ToString();
        string championRiotId = skinRiotIdAsString[..^3];
        
        var identifierPostfix = $"{championRiotId}/{skinRiotId}.jpg";
        
        var combinedPath = string.Concat(CommunityDragonUrl, UncenteredSplashPostfixUrl, identifierPostfix);

        return combinedPath;
    }
    
    public static string GetChampionIconByRiotChampionId(int championRiotId)
    {
        var identifierPostfix = $"{championRiotId}.png";
        
        var combinedPath = string.Concat(CommunityDragonUrl, ChampionIconPostfixUrl, identifierPostfix);

        return combinedPath;
    }
    
    public static string GetChampionTileByRiotChampionId(int championRiotId)
    {
        var identifierPostfix = $"{championRiotId}/{championRiotId}000.jpg";
        
        var combinedPath = string.Concat(CommunityDragonUrl, ChampionTilePostfixUrl, identifierPostfix);

        return combinedPath;
    }
    
    public static string GetChampionSkinTileByRiotChampionId(int skinRiotId)
    {
        string skinRiotIdAsString = skinRiotId.ToString();
        string championRiotId = skinRiotIdAsString[..^3];
        
        var identifierPostfix = $"{championRiotId}/{skinRiotId}.jpg";
        
        var combinedPath = string.Concat(CommunityDragonUrl, ChampionTilePostfixUrl, identifierPostfix);

        return combinedPath;
    }
}