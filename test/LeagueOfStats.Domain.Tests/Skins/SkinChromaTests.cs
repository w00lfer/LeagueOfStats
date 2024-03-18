using LeagueOfStats.Domain.Skins;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Skins;

[TestFixture]
public class SkinChromaTests
{
    [Test]
    public void Constructor_AllValid_CreatesSkinChromaWithProvidedData()
    {
        
        const int riotChromaId = 2;
        const string chromaPath = "chromaPath";
        string[] colorAsStrings = { "red", "blue" };

        Skin skin = Mock.Of<Skin>();

        SkinChroma skinChroma = new SkinChroma(riotChromaId, chromaPath, colorAsStrings, skin);
        
        
        Assert.That(skinChroma.RiotChromaId, Is.EqualTo(riotChromaId));
        Assert.That(skinChroma.ChromaPath, Is.EqualTo(chromaPath));
        Assert.That(skinChroma.ColorsAsStringSeparatedByComma, Is.EqualTo(string.Join(',', colorAsStrings)));
        Assert.That(skinChroma.Skin, Is.EqualTo(skin));
    }
}