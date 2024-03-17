using LeagueOfStats.Domain.Skins;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Skins;

[TestFixture]
public class SkinChromaTests
{
    [Test]
    public void Constructor_AllValid_CreateSkinChromaWithProvidedData()
    {
        
        const int riotSkinId = 1;
        const bool isBase = false;
        const string name = "name";
        const string description = "description";
        const string splashUrl = "splashUrl";
        const string uncenteredSplashUrl = "uncenteredSplashUrl";
        const string tileUrl = "titleUrl";
        const string rarity = "rarity";
        const bool isLegacy = false;
        const string skinChromaPath = "chromaPath";

        const int riotChromaId = 2;
        const string chromaPath = "chromaPath";
        string[] colorAsStrings = { "red", "blue" };
        AddSkinChromaDto addSkinChromaDto = new(riotChromaId, chromaPath, colorAsStrings);
        IEnumerable<AddSkinChromaDto> addSkinChromaDtos = new List<AddSkinChromaDto>
        {
            addSkinChromaDto
        };

        AddSkinDto addSkinDto = new AddSkinDto(
            riotSkinId,
            isBase,
            name,
            description,
            splashUrl,
            uncenteredSplashUrl,
            tileUrl,
            rarity,
            isLegacy,
            skinChromaPath,
            addSkinChromaDtos);
        
        Skin skin = new Skin(addSkinDto);

        SkinChroma skinChroma = new SkinChroma(riotChromaId, chromaPath, colorAsStrings, skin);
        
        
        Assert.That(skinChroma.RiotChromaId, Is.EqualTo(riotChromaId));
        Assert.That(skinChroma.ChromaPath, Is.EqualTo(chromaPath));
        Assert.That(skinChroma.ColorsAsStringSeparatedByComma, Is.EqualTo(string.Join(',', colorAsStrings)));
        Assert.That(skinChroma.Skin, Is.EqualTo(skin));
    }
}