using LeagueOfStats.Domain.Matches.Participants;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Matches.Participants;

[TestFixture]
public class PerksTests
{
    [Test]
    public void Constructor_AllValid_CreatesPerksWithPerkStatsAndStylesWithProvidedData()
    {
        Participant participant = Mock.Of<Participant>();

        const int defense = 1;
        const int flex = 2;
        const int offense = 3;
        AddPerkStatsDto addPerkStatsDto = new(
            defense,
            flex,
            offense);
        
        AddPerksDto addPerksDto = new(
            addPerkStatsDto,
            Enumerable.Empty<AddPerkStyleDto>());

        Perks perks = new(addPerksDto, participant);
        
        Assert.That(perks.Participant, Is.EqualTo(participant));
        Assert.That(perks.StatPerks.Defense, Is.EqualTo(defense));
        Assert.That(perks.StatPerks.Flex, Is.EqualTo(flex));
        Assert.That(perks.StatPerks.Offense, Is.EqualTo(offense));
        Assert.That(perks.StatPerks.Perks, Is.EqualTo(perks));
        Assert.That(perks.Styles.Count, Is.EqualTo(0));
    }
}