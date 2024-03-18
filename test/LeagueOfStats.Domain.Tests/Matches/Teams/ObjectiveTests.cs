using LeagueOfStats.Domain.Matches.Teams;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Matches.Teams;

[TestFixture]
public class ObjectiveTests
{
    [Test]
    public void Constructor_AllValid_CreatesTeamAndBansAndObjectivesWithProvidedData()
    {
        Objectives objectives = Mock.Of<Objectives>();
        
        const bool first = true;
        const int kills = 1;


        AddObjectiveDto addObjectiveDto = new(first, kills);

        Objective objective = new(addObjectiveDto, objectives);
        
        Assert.That(objective.Objectives, Is.EqualTo(objectives));
        Assert.That(objective.First, Is.EqualTo(first));
        Assert.That(objective.Kills, Is.EqualTo(kills));
    }
}