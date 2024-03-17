using LeagueOfStats.Domain.Skins;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Skins;

[TestFixture]
public class SkinImageTests
{
    [Test]
    public void Create_AllValid_CreatSkinImageWithProvidedData()
    {
        const string splashUrl = "splashUrl";
        const string uncenteredSplashUrl = "uncenteredSplashUrl";
        const string tileUrl = "titleUrl";

        SkinImage skinImage = SkinImage.Create(splashUrl, uncenteredSplashUrl, tileUrl);
        
        Assert.That(skinImage.SplashUrl, Is.EqualTo(splashUrl));
        Assert.That(skinImage.UncenteredSplashUrl, Is.EqualTo(uncenteredSplashUrl));
        Assert.That(skinImage.TileUrl, Is.EqualTo(tileUrl));
    }

    [Test]
    public void GetEqualityComponents_AllValid_ReturnsEqualityComponents()
    {
        const string splashUrl = "splashUrl";
        const string uncenteredSplashUrl = "uncenteredSplashUrl";
        const string tileUrl = "titleUrl";

        SkinImage skinImage = SkinImage.Create(splashUrl, uncenteredSplashUrl, tileUrl);

        IEnumerable<object> equalityComponents = skinImage.GetEqualityComponents();
        
        Assert.That(equalityComponents.Count(), Is.EqualTo(3));
        
        Assert.That(equalityComponents.ElementAt(0), Is.EqualTo(splashUrl));
        Assert.That(equalityComponents.ElementAt(1), Is.EqualTo(uncenteredSplashUrl));
        Assert.That(equalityComponents.ElementAt(2), Is.EqualTo(tileUrl));
    }
}