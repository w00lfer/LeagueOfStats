using Camille.Enums;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Application.Summoners.Enums;
using NUnit.Framework;

namespace LeagueOfStats.Application.Tests.Extensions;

[TestFixture]
public class QueueFilterExtensionsTests
{
    [TestCase(MatchHistoryQueueFilter.All, null)]
    [TestCase(MatchHistoryQueueFilter.Custom, Queue.CUSTOM)]
    public void ToNullableQueue_AllValid_ReturnsCorrectQueue(MatchHistoryQueueFilter matchHistoryQueueFilter, Queue? expectedQueue)
    {
        Assert.That(matchHistoryQueueFilter.ToNullableQueue(), Is.EqualTo(expectedQueue));
    }
}