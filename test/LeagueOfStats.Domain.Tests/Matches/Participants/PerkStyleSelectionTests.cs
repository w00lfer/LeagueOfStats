using LeagueOfStats.Domain.Matches.Participants;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Matches.Participants;

public class PerkStyleSelectionTests
{
    [Test]
    public void Constructor_AllValid_CreatesPerkStyleSelectionWithProvidedData()
    {
        PerkStyle perkStyle = Mock.Of<PerkStyle>();

        const int perk = 1;
        const int var1 = 2;
        const int var2 = 3;
        const int var3 = 4;

        AddPerkStyleSelectionDto addPerkStyleSelectionDto = new(
            perk,
            var1,
            var2,
            var3);

        PerkStyleSelection perkStyleSelection = new(addPerkStyleSelectionDto, perkStyle);
        
        Assert.That(perkStyleSelection.PerkStyle, Is.EqualTo(perkStyle));
        Assert.That(perkStyleSelection.Perk, Is.EqualTo(perk));
        Assert.That(perkStyleSelection.Var1, Is.EqualTo(var1));
        Assert.That(perkStyleSelection.Var2, Is.EqualTo(var2));
        Assert.That(perkStyleSelection.Var3, Is.EqualTo(var3));
    }
}