using LeagueOfStats.Domain.Matches.Teams;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Matches.Teams;

[TestFixture]
public class ObjectivesTests
{
    [Test]
    public void Constructor_AllValid_CreatesObjectivesWithProvidedData()
    {
        Team team = Mock.Of<Team>();
        
        const bool baronObjectiveFirst = true;
        const bool championObjectiveFirst = false;
        const bool dragonObjectiveFirst = true;
        const bool inhibitorObjectiveFirst = false;
        const bool riftHeraldObjectiveFirst = true;
        const bool towerObjectiveFirst = false;
        
        const int baronObjectiveKills = 1;
        const int championObjectiveKills = 2;
        const int dragonObjectiveKills = 3;
        const int inhibitorObjectiveKills = 4;
        const int riftHeraldObjectiveKills = 5;
        const int towerObjectiveKills = 6;

        AddObjectiveDto baronAddObjectiveDto = new(baronObjectiveFirst, baronObjectiveKills);
        AddObjectiveDto championAddObjectiveDto = new(championObjectiveFirst, championObjectiveKills);
        AddObjectiveDto dragonAddObjectiveDto = new(dragonObjectiveFirst, dragonObjectiveKills);
        AddObjectiveDto? hordeAddObjectiveDto = null;
        AddObjectiveDto inhibitorAddObjectiveDto = new(inhibitorObjectiveFirst, inhibitorObjectiveKills);
        AddObjectiveDto riftHeraldAddObjectiveDto = new(riftHeraldObjectiveFirst, riftHeraldObjectiveKills);
        AddObjectiveDto towerADdObjectiveDto = new(towerObjectiveFirst, towerObjectiveKills);

        AddObjectivesDto addObjectivesDto = new(
            baronAddObjectiveDto,
            championAddObjectiveDto,
            dragonAddObjectiveDto,
            hordeAddObjectiveDto,
            inhibitorAddObjectiveDto,
            riftHeraldAddObjectiveDto,
            towerADdObjectiveDto);

        Objectives objectives = new(addObjectivesDto, team);
        
        Assert.That(objectives.Team, Is.EqualTo(team));
        
        Assert.That(objectives.BaronObjective.First, Is.EqualTo(baronObjectiveFirst));
        Assert.That(objectives.BaronObjective.Kills, Is.EqualTo(baronObjectiveKills));
        Assert.That(objectives.BaronObjective.Objectives, Is.EqualTo(objectives));
        
        Assert.That(objectives.ChampionObjective.First, Is.EqualTo(championObjectiveFirst));
        Assert.That(objectives.ChampionObjective.Kills, Is.EqualTo(championObjectiveKills));
        Assert.That(objectives.ChampionObjective.Objectives, Is.EqualTo(objectives));
        
        Assert.That(objectives.DragonObjective.First, Is.EqualTo(dragonObjectiveFirst));
        Assert.That(objectives.DragonObjective.Kills, Is.EqualTo(dragonObjectiveKills));
        Assert.That(objectives.DragonObjective.Objectives, Is.EqualTo(objectives));
        
        Assert.That(objectives.HordeObjective, Is.Null);
        
        Assert.That(objectives.InhibitorObjective.First, Is.EqualTo(inhibitorObjectiveFirst));
        Assert.That(objectives.InhibitorObjective.Kills, Is.EqualTo(inhibitorObjectiveKills));
        Assert.That(objectives.InhibitorObjective.Objectives, Is.EqualTo(objectives));
        
        Assert.That(objectives.RiftHeraldObjective.First, Is.EqualTo(riftHeraldObjectiveFirst));
        Assert.That(objectives.RiftHeraldObjective.Kills, Is.EqualTo(riftHeraldObjectiveKills));
        Assert.That(objectives.RiftHeraldObjective.Objectives, Is.EqualTo(objectives));
        
        Assert.That(objectives.TowerObjective.First, Is.EqualTo(towerObjectiveFirst));
        Assert.That(objectives.TowerObjective.Kills, Is.EqualTo(towerObjectiveKills));
        Assert.That(objectives.TowerObjective.Objectives, Is.EqualTo(objectives));
    }
}