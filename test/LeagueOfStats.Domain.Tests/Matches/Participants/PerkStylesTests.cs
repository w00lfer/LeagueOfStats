using LeagueOfStats.Domain.Matches.Participants;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Matches.Participants;

[TestFixture]
public class PerkStylesTests
{
    [Test]
    public void Constructor_AllValid_CreatesPerkStylesWithPerkStyleSelectionsWithProvidedData()
    {
        Perks perks = Mock.Of<Perks>();

        const string description = "description";
        const int style = 1;

        AddPerkStyleDto addPerkStyleDto = new(
            Enumerable.Empty<AddPerkStyleSelectionDto>(),
            description,
            style);

        PerkStyle perkStyle = new(addPerkStyleDto, perks);
        
        Assert.That(perkStyle.Perks, Is.EqualTo(perks));
        Assert.That(perkStyle.Description, Is.EqualTo(description));
        Assert.That(perkStyle.Style, Is.EqualTo(style));
        Assert.That(perkStyle.Selections.Count, Is.EqualTo(0));
    }
}