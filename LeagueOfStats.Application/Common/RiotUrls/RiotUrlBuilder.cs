namespace LeagueOfStats.Application.Common.RiotUrls;

public static class RiotUrlBuilder
{
    private const string CommunityDragonUrl = "raw.communitydragon.org/latest/";
    private const string CommunityDragonUrlForChampionSplash = "raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-splashes/";
    
    private const string LeaguePathToReplace = "lol-game-data/assets/";
    private const string LeagueUrlToReplace = "plugins/rcp-be-lol-game-data/global/default/";
    
    public static string MapFromLeagueClientPathToCommunityDragonUrlForSkin(string clientPath)
    {
        var pathTransformedToUrl = clientPath.Replace(LeaguePathToReplace, LeagueUrlToReplace);

        var combinedPath = string.Concat(CommunityDragonUrl, pathTransformedToUrl);

        return combinedPath;
    }
    
    public static string MapFromLeagueClientPathToCommunityDragonUrlForChampion(int championRiotId)
    {
        var pathForChampion = string.Format("{0}/{0}000.jpg", championRiotId);
        
        var combinedPath = string.Concat(CommunityDragonUrlForChampionSplash, pathForChampion);

        return combinedPath;
    }
}