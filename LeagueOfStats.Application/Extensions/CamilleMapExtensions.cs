using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Matches.Enums;

namespace LeagueOfStats.Application.Extensions;

public static class CamilleMapExtensions
{
    public static Map ToMap(this Camille.Enums.Map map) =>
        map switch
        {
            Camille.Enums.Map.SUMMONERS_RIFT_ORIGINAL_SUMMER_VARIANT => Map.SummonersRiftOriginalSummerVariant,
            Camille.Enums.Map.SUMMONERS_RIFT_ORIGINAL_AUTUMN_VARIANT => Map.SummonersRiftOriginalAutumnVariant,
            Camille.Enums.Map.THE_PROVING_GROUNDS => Map.TheProvingGrounds,
            Camille.Enums.Map.TWISTED_TREELINE_ORIGINAL_VERSION => Map.TwistedTreelineOriginalVersion,
            Camille.Enums.Map.THE_CRYSTAL_SCAR => Map.TheCrystalScar,
            Camille.Enums.Map.TWISTED_TREELINE => Map.TwistedTreeline,
            Camille.Enums.Map.SUMMONERS_RIFT => Map.SummonersRift,
            Camille.Enums.Map.HOWLING_ABYSS => Map.HowlingAbyss,
            Camille.Enums.Map.BUTCHERS_BRIDGE => Map.ButchersBridge,
            Camille.Enums.Map.COSMIC_RUINS => Map.CosmicRuins,
            Camille.Enums.Map.VALORAN_CITY_PARK => Map.ValoranCityPark,
            Camille.Enums.Map.SUBSTRUCTURE_43 => Map.Substructure43,
            Camille.Enums.Map.CRASH_SITE => Map.CrashSite,
            Camille.Enums.Map.NEXUS_BLITZ => Map.NexusBlitz,
            Camille.Enums.Map.CONVERGENCE => Map.Convergence,
            Camille.Enums.Map.ARENA => Map.Arena,
            _ => throw new ArgumentOutOfRangeException(nameof(map), map, null)
        };
}