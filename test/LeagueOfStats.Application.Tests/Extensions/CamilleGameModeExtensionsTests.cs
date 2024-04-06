using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Domain.Matches.Enums;
using NUnit.Framework;

namespace LeagueOfStats.Application.Tests.Extensions;

[TestFixture]
public class CamilleGameModeExtensionsTests
{
    [TestCase(Camille.Enums.GameMode.ARAM, GameMode.Aram)]
    [TestCase(Camille.Enums.GameMode.ARSR, GameMode.Arsr)]
    [TestCase(Camille.Enums.GameMode.ASCENSION, GameMode.Ascension)]
    [TestCase(Camille.Enums.GameMode.ASSASSINATE, GameMode.Assassinate)]
    [TestCase(Camille.Enums.GameMode.CHERRY, GameMode.Arena)]
    [TestCase(Camille.Enums.GameMode.CLASSIC, GameMode.Classic)]
    [TestCase(Camille.Enums.GameMode.DARKSTAR, GameMode.DarkStar)]
    [TestCase(Camille.Enums.GameMode.DOOMBOTSTEEMO, GameMode.DoomBots)]
    [TestCase(Camille.Enums.GameMode.FIRSTBLOOD, GameMode.FirstBlood)]
    [TestCase(Camille.Enums.GameMode.GAMEMODEX, GameMode.GameModeX)]
    [TestCase(Camille.Enums.GameMode.KINGPORO, GameMode.KingPoro)]
    [TestCase(Camille.Enums.GameMode.NEXUSBLITZ, GameMode.NexusBlitz)]
    [TestCase(Camille.Enums.GameMode.ODIN, GameMode.Odin)]
    [TestCase(Camille.Enums.GameMode.ODYSSEY, GameMode.Odyssey)]
    [TestCase(Camille.Enums.GameMode.ONEFORALL, GameMode.OneForAll)]
    [TestCase(Camille.Enums.GameMode.PRACTICETOOL, GameMode.PracticeTool)]
    [TestCase(Camille.Enums.GameMode.PROJECT, GameMode.Project)]
    [TestCase(Camille.Enums.GameMode.SIEGE, GameMode.Siege)]
    [TestCase(Camille.Enums.GameMode.STARGUARDIAN, GameMode.StarGuardian)]
    [TestCase(Camille.Enums.GameMode.TUTORIAL, GameMode.Tutorial)]
    [TestCase(Camille.Enums.GameMode.TUTORIAL_MODULE_1, GameMode.TutorialModule1)]
    [TestCase(Camille.Enums.GameMode.TUTORIAL_MODULE_2, GameMode.TutorialModule2)]
    [TestCase(Camille.Enums.GameMode.TUTORIAL_MODULE_3, GameMode.TutorialModule3)]
    [TestCase(Camille.Enums.GameMode.ULTBOOK, GameMode.Ultbook)]
    [TestCase(Camille.Enums.GameMode.URF, GameMode.Urf)]
    public void ToGameMode_AllValid_ReturnsCorrectDomainGameMode(Camille.Enums.GameMode gameMode, GameMode expectedGameMode)
    {
        Assert.That(gameMode.ToGameMode(), Is.EqualTo(expectedGameMode));
    }
}