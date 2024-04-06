using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Domain.Matches.Enums;
using NUnit.Framework;

namespace LeagueOfStats.Application.Tests.Extensions;

[TestFixture]
public class CamilleGameTypeExtensionsTests
{
    [TestCase(Camille.Enums.GameType.CUSTOM_GAME, GameType.CustomGame)]
    [TestCase(Camille.Enums.GameType.MATCHED_GAME, GameType.MatchedGame)]
    [TestCase(Camille.Enums.GameType.TUTORIAL_GAME, GameType.TutorialGame)]
    public void ToGameType_AllValid_ReturnsCorrectDomainGameType(Camille.Enums.GameType gameType, GameType expectedGameType)
    {
        Assert.That(gameType.ToGameType(), Is.EqualTo(expectedGameType));
    }
}