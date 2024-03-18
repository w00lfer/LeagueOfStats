using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Matches.Teams;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Matches.Teams;

[TestFixture]
public class BanTests
{
    [Test]
    public void Constructor_AllValid_CreatesTeamAndBansAndObjectivesWithProvidedData()
    {
        Team team = Mock.Of<Team>();

        Guid championId = Guid.NewGuid();
        Champion champion = Mock.Of<Champion>(c => c.Id == championId);
        const int pickTurn = 1;

        AddBanDto addBanDto = new(champion, pickTurn);

        Ban ban = new(addBanDto, team);
        
        Assert.That(ban.Team, Is.EqualTo(team));
        Assert.That(ban.ChampionId, Is.EqualTo(championId));
        Assert.That(ban.PickTurn, Is.EqualTo(pickTurn));
    }
}