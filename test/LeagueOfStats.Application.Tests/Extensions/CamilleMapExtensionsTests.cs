using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Domain.Matches.Enums;
using NUnit.Framework;

namespace LeagueOfStats.Application.Tests.Extensions;

[TestFixture]
public class CamilleMapExtensionsTests
{
    [TestCase(Camille.Enums.Map.SUMMONERS_RIFT_ORIGINAL_SUMMER_VARIANT, Map.SummonersRiftOriginalSummerVariant)]
    [TestCase(Camille.Enums.Map.SUMMONERS_RIFT_ORIGINAL_AUTUMN_VARIANT, Map.SummonersRiftOriginalAutumnVariant)]
    [TestCase(Camille.Enums.Map.THE_PROVING_GROUNDS, Map.TheProvingGrounds)]
    [TestCase(Camille.Enums.Map.TWISTED_TREELINE_ORIGINAL_VERSION, Map.TwistedTreelineOriginalVersion)]
    [TestCase(Camille.Enums.Map.THE_CRYSTAL_SCAR, Map.TheCrystalScar)]
    [TestCase(Camille.Enums.Map.TWISTED_TREELINE, Map.TwistedTreeline)]
    [TestCase(Camille.Enums.Map.SUMMONERS_RIFT, Map.SummonersRift)]
    [TestCase(Camille.Enums.Map.HOWLING_ABYSS, Map.HowlingAbyss)]
    [TestCase(Camille.Enums.Map.BUTCHERS_BRIDGE, Map.ButchersBridge)]
    [TestCase(Camille.Enums.Map.COSMIC_RUINS, Map.CosmicRuins)]
    [TestCase(Camille.Enums.Map.VALORAN_CITY_PARK, Map.ValoranCityPark)]
    [TestCase(Camille.Enums.Map.SUBSTRUCTURE_43, Map.Substructure43)]
    [TestCase(Camille.Enums.Map.CRASH_SITE, Map.CrashSite)]
    [TestCase(Camille.Enums.Map.NEXUS_BLITZ, Map.NexusBlitz)]
    [TestCase(Camille.Enums.Map.CONVERGENCE, Map.Convergence)]
    [TestCase(Camille.Enums.Map.ARENA, Map.Arena)]
    public void ToMap_AllValid_ReturnsCorrectDomainMap(Camille.Enums.Map map, Map expectedMap)
    {
        Assert.That(map.ToMap(), Is.EqualTo(expectedMap));
    }
}