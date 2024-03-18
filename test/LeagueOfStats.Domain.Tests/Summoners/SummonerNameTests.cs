using LeagueOfStats.Domain.Summoners;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Summoners;

[TestFixture]
public class SummonerNameTests
{
    [Test]
    public void Create_AllValid_CreatSummonerNameWithProvidedData()
    {
        const string gameName = "gameName";
        const string tagLine = "tagLine";

        SummonerName summonerName = SummonerName.Create(gameName, tagLine);
        
        Assert.That(summonerName.GameName, Is.EqualTo(gameName));
        Assert.That(summonerName.TagLine, Is.EqualTo(tagLine));
    }

    [Test]
    public void GetEqualityComponents_AllValid_ReturnsEqualityComponents()
    {
        const string gameName = "gameName";
        const string tagLine = "tagLine";

        SummonerName summonerName = SummonerName.Create(gameName, tagLine);

        IEnumerable<object> equalityComponents = summonerName.GetEqualityComponents();
        
        Assert.That(equalityComponents.Count(), Is.EqualTo(2));
        
        Assert.That(equalityComponents.ElementAt(0), Is.EqualTo(gameName));
        Assert.That(equalityComponents.ElementAt(1), Is.EqualTo(tagLine));
    }
}