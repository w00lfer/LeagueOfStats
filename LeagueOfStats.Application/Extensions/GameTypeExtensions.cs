using Camille.Enums;
using GameType = LeagueOfStats.Domain.Common.Enums.GameType;

namespace LeagueOfStats.Application.Extensions;

public static class GameTypeExtensions
{
    public static Queue? ToNullableQueue(this GameType gameType) =>
        gameType switch
        {
            GameType.All => null,
            GameType.Custom => Queue.CUSTOM,
            _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
        };
}