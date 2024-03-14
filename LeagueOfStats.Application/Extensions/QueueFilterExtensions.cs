using Camille.Enums;
using LeagueOfStats.Application.Summoners.Enums;

namespace LeagueOfStats.Application.Extensions;

public static class QueueFilterExtensions
{
    public static Queue? ToNullableQueue(this MatchHistoryQueueFilter matchHistoryQueueFilter) =>
        matchHistoryQueueFilter switch
        {
            MatchHistoryQueueFilter.All => null,
            MatchHistoryQueueFilter.Custom => Queue.CUSTOM,
            _ => throw new ArgumentOutOfRangeException(nameof(matchHistoryQueueFilter), matchHistoryQueueFilter, null)
        };
}