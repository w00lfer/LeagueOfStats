using LeagueOfStats.Domain.Champions;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Champions;

[TestFixture]
public class ChampionImageTests
{
    [Test]
    public void Create_AllValid_CreateChampionImageWithProvidedData()
    {
        const string splashUrl = "splashUrl";
        const string uncenteredSplashUrl = "uncenteredSplashUrl";
        const string iconUrl = "iconUrl";
        const string tileUrl = "titleUrl";

        ChampionImage championImage = ChampionImage.Create(splashUrl, uncenteredSplashUrl, iconUrl, tileUrl);
        
        Assert.That(championImage.SplashUrl, Is.EqualTo(splashUrl));
        Assert.That(championImage.UncenteredSplashUrl, Is.EqualTo(uncenteredSplashUrl));
        Assert.That(championImage.IconUrl, Is.EqualTo(iconUrl));
        Assert.That(championImage.TileUrl, Is.EqualTo(tileUrl));
    }

    [Test]
    public void GetEqualityComponents_AllValid_ReturnsEqualityComponents()
    {
        const string splashUrl = "splashUrl";
        const string uncenteredSplashUrl = "uncenteredSplashUrl";
        const string iconUrl = "iconUrl";
        const string tileUrl = "titleUrl";

        ChampionImage championImage = ChampionImage.Create(splashUrl, uncenteredSplashUrl, iconUrl, tileUrl);

        IEnumerable<object> equalityComponents = championImage.GetEqualityComponents();
        
        Assert.That(equalityComponents.Count(), Is.EqualTo(4));
        
        Assert.That(equalityComponents.ElementAt(0), Is.EqualTo(splashUrl));
        Assert.That(equalityComponents.ElementAt(1), Is.EqualTo(uncenteredSplashUrl));
        Assert.That(equalityComponents.ElementAt(2), Is.EqualTo(iconUrl));
        Assert.That(equalityComponents.ElementAt(3), Is.EqualTo(tileUrl));
    }
}