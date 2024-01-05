using Camille.Enums;
using LeagueOfStats.Application.Common.Enums;

namespace LeagueOfStats.Application.Extensions;

public static class QueueFilterExtensions
{
    public static Queue? ToNullableQueue(this QueueFilter queueFilter) =>
        queueFilter switch
        {
            QueueFilter.All => null,
            QueueFilter.Custom => Queue.CUSTOM,
            _ => throw new ArgumentOutOfRangeException(nameof(queueFilter), queueFilter, null)
        };
}