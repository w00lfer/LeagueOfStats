using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Matches.Enums;

namespace LeagueOfStats.Application.Extensions;

public static class CamilleGameModeExtensions
{
    public static GameMode ToGameMode(this Camille.Enums.GameMode gameMode) =>
        gameMode switch
        {
            Camille.Enums.GameMode.ARAM => GameMode.Aram,
            Camille.Enums.GameMode.ARSR => GameMode.Arsr,
            Camille.Enums.GameMode.ASCENSION => GameMode.Ascension,
            Camille.Enums.GameMode.ASSASSINATE => GameMode.Assassinate,
            Camille.Enums.GameMode.CHERRY => GameMode.Arena,
            Camille.Enums.GameMode.CLASSIC => GameMode.Classic,
            Camille.Enums.GameMode.DARKSTAR => GameMode.DarkStar,
            Camille.Enums.GameMode.DOOMBOTSTEEMO => GameMode.DoomBots,
            Camille.Enums.GameMode.FIRSTBLOOD => GameMode.FirstBlood,
            Camille.Enums.GameMode.GAMEMODEX => GameMode.GameModeX,
            Camille.Enums.GameMode.KINGPORO => GameMode.KingPoro,
            Camille.Enums.GameMode.NEXUSBLITZ => GameMode.NexusBlitz,
            Camille.Enums.GameMode.ODIN => GameMode.Odin,
            Camille.Enums.GameMode.ODYSSEY => GameMode.Odyssey,
            Camille.Enums.GameMode.ONEFORALL => GameMode.OneForAll,
            Camille.Enums.GameMode.PRACTICETOOL => GameMode.PracticeTool,
            Camille.Enums.GameMode.PROJECT => GameMode.Project,
            Camille.Enums.GameMode.SIEGE => GameMode.Siege,
            Camille.Enums.GameMode.STARGUARDIAN => GameMode.StarGuardian,
            Camille.Enums.GameMode.TUTORIAL => GameMode.Tutorial,
            Camille.Enums.GameMode.TUTORIAL_MODULE_1 => GameMode.TutorialModule1,
            Camille.Enums.GameMode.TUTORIAL_MODULE_2 => GameMode.TutorialModule2,
            Camille.Enums.GameMode.TUTORIAL_MODULE_3 => GameMode.TutorialModule3,
            Camille.Enums.GameMode.ULTBOOK => GameMode.Ultbook,
            Camille.Enums.GameMode.URF => GameMode.Urf,
            _ => throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null)
        };
}