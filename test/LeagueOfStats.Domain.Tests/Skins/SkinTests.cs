using LeagueOfStats.Domain.Skins;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Skins;

[TestFixture]
public class SkinTests
{
    [Test]
    public void Constructor_AllValid_CreateSkinAndSkinImageAndSkinChromasWithProvidedData()
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
        Assert.That(skin.RiotSkinId, Is.EqualTo(riotSkinId));
        Assert.That(skin.IsBase, Is.EqualTo(isBase));
        Assert.That(skin.Name, Is.EqualTo(name));
        Assert.That(skin.Description, Is.EqualTo(description));
        Assert.That(skin.Rarity, Is.EqualTo(rarity));
        Assert.That(skin.IsLegacy, Is.EqualTo(isLegacy));
        Assert.That(skin.ChromaPath, Is.EqualTo(chromaPath));
        
        Assert.That(skin.SkinImage.SplashUrl, Is.EqualTo(splashUrl));
        Assert.That(skin.SkinImage.UncenteredSplashUrl, Is.EqualTo(uncenteredSplashUrl));
        Assert.That(skin.SkinImage.TileUrl, Is.EqualTo(tileUrl));
        
        Assert.That(skin.Chromas.Count, Is.EqualTo(1));
        
        SkinChroma skinChroma = skin.Chromas.ElementAt(0);
        Assert.That(skinChroma.RiotChromaId, Is.EqualTo(riotChromaId));
        Assert.That(skinChroma.ChromaPath, Is.EqualTo(chromaPath));
        Assert.That(skinChroma.ColorsAsStringSeparatedByComma, Is.EqualTo(string.Join(',', colorAsStrings)));
    }
}