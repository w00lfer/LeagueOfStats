using LeagueOfStats.Jobs.Common;
using NUnit.Framework;

namespace LeagueOfStats.Jobs.Tests.Common;

public class RiotUrlBuilderTests
{
    [Test]
    public void GetChampionSplashByRiotChampionId_AllValid_ReturnsUrl()
    {
        var championRiotId = 50;

        var url = RiotUrlBuilder.GetChampionSplashByRiotChampionId(championRiotId);
        
        var expectedUrl = "raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-splashes/50/50000.jpg";
        
        Assert.That(url, Is.EqualTo(expectedUrl));
    }
    
    [Test]
    public void GetChampionSkinSplashByRiotChampionId_AllValid_ReturnsUrl()
    {
        var skinId = 45000;

        var url = RiotUrlBuilder.GetChampionSkinSplashByRiotChampionId(skinId);
        
        var expectedUrl = "raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-splashes/45/45000.jpg";
        
        Assert.That(url, Is.EqualTo(expectedUrl));
    }
    
    [Test]
    public void GetChampionUncenteredSplashByRiotChampionId_AllValid_ReturnsUrl()
    {
        var championRiotId = 50;

        var url = RiotUrlBuilder.GetChampionUncenteredSplashByRiotChampionId(championRiotId);
        
        var expectedUrl = "raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-splashes/uncentered/50/50000.jpg";
        
        Assert.That(url, Is.EqualTo(expectedUrl));
    }
    
    [Test]
    public void GetChampionSkinUncenteredSplashByRiotChampionId_AllValid_ReturnsUrl()
    {
        var skinId = 45000;

        var url = RiotUrlBuilder.GetChampionSkinUncenteredSplashByRiotChampionId(skinId);
        
        var expectedUrl = "raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-splashes/uncentered/45/45000.jpg";
        
        Assert.That(url, Is.EqualTo(expectedUrl));
    }
    
    [Test]
    public void GetChampionIconByRiotChampionId_AllValid_ReturnsUrl()
    {
        var championRiotId = 50;

        var url = RiotUrlBuilder.GetChampionIconByRiotChampionId(championRiotId);
        
        var expectedUrl = "raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-icons/50.png";
        
        Assert.That(url, Is.EqualTo(expectedUrl));
    }
    
    [Test]
    public void GetChampionTileByRiotChampionId_AllValid_ReturnsUrl()
    {
        var championRiotId = 50;

        var url = RiotUrlBuilder.GetChampionTileByRiotChampionId(championRiotId);
        
        var expectedUrl = "raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-tiles/50/50000.jpg";
        
        Assert.That(url, Is.EqualTo(expectedUrl));
    }
    
    [Test]
    public void GetChampionSkinTileByRiotChampionId_AllValid_ReturnsUrl()
    {
        var skinId = 45000;

        var url = RiotUrlBuilder.GetChampionSkinTileByRiotChampionId(skinId);
        
        var expectedUrl = "raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/champion-tiles/45/45000.jpg";
        
        Assert.That(url, Is.EqualTo(expectedUrl));
    }
}