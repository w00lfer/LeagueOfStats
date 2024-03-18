using LeagueOfStats.Domain.Matches.Teams;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using LeagueOfStats.Domain.Matches.Teams.Enums;
using Moq;
using NUnit.Framework;
using Match = LeagueOfStats.Domain.Matches.Match;

namespace LeagueOfStats.Domain.Tests.Matches.Teams;

[TestFixture]
public class TeamTests
{
    [Test]
    public void Constructor_AllValid_CreatesTeamAndBansAndObjectivesWithProvidedData()
    {
        Match match = Mock.Of<Match>();
        const Side side = Side.Red;
        const bool win = true;

        AddTeamDto addTeamDto = new AddTeamDto(
            new AddObjectivesDto(
                new AddObjectiveDto(false, 0),
                new AddObjectiveDto(false, 0),
                new AddObjectiveDto(false, 0),
                new AddObjectiveDto(false, 0),
                new AddObjectiveDto(false, 0),
                new AddObjectiveDto(false, 0),
                new AddObjectiveDto(false, 0)),
            Enumerable.Empty<AddBanDto>(),
            side,
            win);

        Team team = new(addTeamDto, match);
        
        Assert.That(team.Match, Is.EqualTo(match));
        Assert.That(team.Side, Is.EqualTo(side));
        Assert.That(team.Win, Is.EqualTo(win));
    }
}