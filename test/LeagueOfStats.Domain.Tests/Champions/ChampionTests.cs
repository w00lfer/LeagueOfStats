using LeagueOfStats.Domain.Champions;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Champions;

[TestFixture]
public class ChampionTests
{
    [Test]
    public void Constructor_AllValid_CreateChampionAndChampionImageWithProvidedData()
    {
        const int riotChampionId = 1;
        const string name = "name";
        const string title = "title";
        const string description = "description";
        const string splashUrl = "splashUrl";
        const string uncenteredSplashUrl = "uncenteredSplashUrl";
        const string iconUrl = "iconUrl";
        const string tileUrl = "titleUrl";

        Champion champion = new Champion(riotChampionId, name, title, description, splashUrl, uncenteredSplashUrl, iconUrl, tileUrl);
        
        Assert.That(champion.RiotChampionId, Is.EqualTo(riotChampionId));
        Assert.That(champion.Name, Is.EqualTo(name));
        Assert.That(champion.Title, Is.EqualTo(title));
        Assert.That(champion.Description, Is.EqualTo(description));
        
        Assert.That(champion.ChampionImage.SplashUrl, Is.EqualTo(splashUrl));
        Assert.That(champion.ChampionImage.UncenteredSplashUrl, Is.EqualTo(uncenteredSplashUrl));
        Assert.That(champion.ChampionImage.IconUrl, Is.EqualTo(iconUrl));
        Assert.That(champion.ChampionImage.TileUrl, Is.EqualTo(tileUrl));
    }
}