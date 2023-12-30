using LeagueOfStats.Domain.Matches;

namespace LeagueOfStats.Application.Extensions;

public static class CamilleGameTypeExtensions
{
    public static GameType ToGameType(this Camille.Enums.GameType gameType) =>
        gameType switch
        {
            Camille.Enums.GameType.CUSTOM_GAME => GameType.CustomGame,
            Camille.Enums.GameType.MATCHED_GAME => GameType.MatchedGame,
            Camille.Enums.GameType.TUTORIAL_GAME => GameType.TutorialGame,
            _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
        };
}