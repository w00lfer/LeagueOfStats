using NodaTime.Text;

namespace LeagueOfStats.Application.Common.NodaTimeHelpers;

public static class LocalDateTimePatternHelper
{
    static LocalDateTimePatternHelper()
    {
        RiotLocalDateTimePattern = LocalDateTimePattern.CreateWithInvariantCulture("uuuu'-'MM'-'dd'-'HH'-'mm'-'ss");
    }

    public static LocalDateTimePattern RiotLocalDateTimePattern { get; }
}