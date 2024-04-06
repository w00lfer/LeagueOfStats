using LeagueOfStats.Application.Common.NodaTimeHelpers;
using NodaTime.Text;
using NUnit.Framework;

namespace LeagueOfStats.Application.Tests.Common.NodaTimeHelpers;

[TestFixture]
public class LocalDateTimePatternHelperTests
{
    [Test]
    public void RiotLocalDateTimePattern_AllValid_ReturnsLocalDateTimePatternForDataReturnedFromRiotShopClient()
    {
        LocalDateTimePattern localDateTimePattern = LocalDateTimePatternHelper.RiotLocalDateTimePattern;
        
        Assert.That(localDateTimePattern.PatternText, Is.EqualTo("uuuu'-'MM'-'dd'-'HH'-'mm'-'ss"));
    }
}