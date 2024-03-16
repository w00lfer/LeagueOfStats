using Camille.Enums;
using LeagueOfStats.Domain.Common.Enums;

namespace LeagueOfStats.Application.Extensions;

public static class RegionExtensions
{
    public static PlatformRoute ToPlatformRoute(this Region region) =>
        region switch
        {
            Region.BR => PlatformRoute.BR1,
            Region.EUNE => PlatformRoute.EUN1,
            Region.EUW => PlatformRoute.EUW1,
            Region.KR => PlatformRoute.KR,
            Region.LAN => PlatformRoute.LA1,
            Region.LAS => PlatformRoute.LA2,
            Region.NA => PlatformRoute.NA1,
            Region.OCE => PlatformRoute.OC1,
            Region.TR => PlatformRoute.TR1,
            Region.RU => PlatformRoute.RU,
            _ => throw new ArgumentOutOfRangeException(nameof(region), region, null)
        };

    public static RegionalRoute ToRegionalRoute(this Region region) =>
        region switch
        {
            Region.BR => RegionalRoute.AMERICAS,
            Region.EUNE => RegionalRoute.EUROPE,
            Region.EUW => RegionalRoute.EUROPE,
            Region.KR => RegionalRoute.ASIA,
            Region.LAN => RegionalRoute.AMERICAS,
            Region.LAS => RegionalRoute.AMERICAS,
            Region.NA => RegionalRoute.AMERICAS,
            Region.OCE => throw new ArgumentOutOfRangeException(nameof(region), region, null),
            Region.TR => throw new ArgumentOutOfRangeException(nameof(region), region, null),
            Region.RU => RegionalRoute.EUROPE,
            _ => throw new ArgumentOutOfRangeException(nameof(region), region, null)
        };
}