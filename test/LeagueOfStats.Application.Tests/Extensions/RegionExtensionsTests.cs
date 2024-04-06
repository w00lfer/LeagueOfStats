using Camille.Enums;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Domain.Common.Enums;
using NUnit.Framework;

namespace LeagueOfStats.Application.Tests.Extensions;

[TestFixture]
public class RegionExtensionsTests
{
    [TestCase(Region.BR, PlatformRoute.BR1)]
    [TestCase(Region.EUNE, PlatformRoute.EUN1)]
    [TestCase(Region.EUW, PlatformRoute.EUW1)]
    [TestCase(Region.KR, PlatformRoute.KR)]
    [TestCase(Region.LAN, PlatformRoute.LA1)]
    [TestCase(Region.LAS, PlatformRoute.LA2)]
    [TestCase(Region.NA, PlatformRoute.NA1)]
    [TestCase(Region.OCE, PlatformRoute.OC1)]
    [TestCase(Region.TR, PlatformRoute.TR1)]
    [TestCase(Region.RU, PlatformRoute.RU)]
    public void ToPlatformRoute_AllValid_ReturnsCorrectPlatformRoute(Region region, PlatformRoute expectedPlatformRoute)
    {
        Assert.That(region.ToPlatformRoute(), Is.EqualTo(expectedPlatformRoute));
    }
    
    [TestCase(Region.BR, RegionalRoute.AMERICAS)]
    [TestCase(Region.EUNE, RegionalRoute.EUROPE)]
    [TestCase(Region.EUW, RegionalRoute.EUROPE)]
    [TestCase(Region.KR, RegionalRoute.ASIA)]
    [TestCase(Region.LAN, RegionalRoute.AMERICAS)]
    [TestCase(Region.LAS, RegionalRoute.AMERICAS)]
    [TestCase(Region.NA, RegionalRoute.AMERICAS)]
    [TestCase(Region.RU, RegionalRoute.EUROPE)]
    public void ToRegionalRoute_AllValid_ReturnsCorrectRegionalRoute(Region region, RegionalRoute expectedRegionalRoute)
    {
        Assert.That(region.ToRegionalRoute(), Is.EqualTo(expectedRegionalRoute));
    }

    [TestCase(Region.OCE)]
    [TestCase(Region.TR)]
    public void ToRegionalRoute_NotSupportedRegions_ThrowsArgumentOutOfRangeException(Region region)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => region.ToRegionalRoute());
    }
}