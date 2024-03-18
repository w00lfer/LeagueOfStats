using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Matches.Enums;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using NodaTime;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Matches;

[TestFixture]
public class MatchTests
{
    [Test]
    public void Constructor_AllValid_CreatesMatchAndParticipantsAndTeamsWithProvidedData()
    {
        const string riotMatchId = "riotMatchId";
        const string gameVersion = "gameVersion";
        Duration gameDuration = Duration.MaxValue;
        Instant gameStartTimeStamp = Instant.MinValue;
        Instant gameEndTimeStamp = Instant.MaxValue;
        const GameMode gameMode = GameMode.Aram;
        const GameType gameType = GameType.MatchedGame;
        const Map map = Map.HowlingAbyss;
        const string platformId = "platformId";
        const Queue queue = Queue.HowlingAbyss5V5Aram;
        const string? tournamentCode = null;

        AddMatchDto addMatchDto = new(
            riotMatchId,
            gameVersion,
            gameDuration,
            gameStartTimeStamp,
            gameEndTimeStamp,
            gameMode,
            gameType,
            map,
            platformId,
            queue,
            tournamentCode,
            Enumerable.Empty<AddParticipantDto>(),
            Enumerable.Empty<AddTeamDto>());

        Match match = new Match(addMatchDto);
        
        Assert.That(match.RiotMatchId, Is.EqualTo(riotMatchId));
        Assert.That(match.GameVersion, Is.EqualTo(gameVersion));
        Assert.That(match.GameDuration, Is.EqualTo(gameDuration));
        Assert.That(match.GameStartTimeStamp, Is.EqualTo(gameStartTimeStamp));
        Assert.That(match.GameEndTimestamp, Is.EqualTo(gameEndTimeStamp));
        Assert.That(match.GameMode, Is.EqualTo(gameMode));
        Assert.That(match.GameType, Is.EqualTo(gameType));
        Assert.That(match.Map, Is.EqualTo(map));
        Assert.That(match.PlatformId, Is.EqualTo(platformId));
        Assert.That(match.Queue, Is.EqualTo(queue));
        Assert.That(match.TournamentCode, Is.EqualTo(tournamentCode));
        Assert.That(match.Participants.Count, Is.EqualTo(0));
        Assert.That(match.Teams.Count, Is.EqualTo(0));
    }
}