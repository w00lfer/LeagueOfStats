using Camille.RiotGames.Enums;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Domain.Matches.Teams.Enums;
using NUnit.Framework;

namespace LeagueOfStats.Application.Tests.Extensions;

[TestFixture]
public class CamilleTeamExtensionsTests
{
    [TestCase(Team.Blue, Side.Blue)]
    [TestCase(Team.Red, Side.Red)]
    [TestCase(Team.Other, Side.Other)]
    [TestCase(0, Side.Arena)]
    public void ToSide_AllValid_ReturnsCorrectDomainSide(Team team, Side expectedSide)
    {
        Assert.That(team.ToSide(), Is.EqualTo(expectedSide));
    }
}