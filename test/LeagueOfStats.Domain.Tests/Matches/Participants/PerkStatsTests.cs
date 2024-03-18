using LeagueOfStats.Domain.Matches.Participants;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Matches.Participants;

[TestFixture]
public class PerkStatsTests
{
    [Test]
    public void Constructor_AllValid_CreatesPerkStatsWithProvidedData()
    {
        Perks perks = Mock.Of<Perks>();

        const int defense = 1;
        const int flex = 2;
        const int offense = 3;
        AddPerkStatsDto addPerkStatsDto = new(
            defense,
            flex,
            offense);
        

        PerkStats perkStats = new(addPerkStatsDto, perks);
        
        Assert.That(perkStats.Defense, Is.EqualTo(defense));
        Assert.That(perkStats.Flex, Is.EqualTo(flex));
        Assert.That(perkStats.Offense, Is.EqualTo(offense));
        Assert.That(perkStats.Perks, Is.EqualTo(perks));
    }
}