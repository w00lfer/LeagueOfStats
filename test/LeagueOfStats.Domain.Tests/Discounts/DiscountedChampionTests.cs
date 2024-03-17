using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Discounts;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Discounts;

public class DiscountedChampionTests
{
    [Test]
    public void Constructor_AllValid_CreateDiscountedChampionWithProvidedData()
    {
        const int oldPrice = 10;
        const int newPrice = 5;
        Guid championId = Guid.NewGuid();
        Champion champion = Mock.Of<Champion>(c => c.Id == championId);
        Discount discount = Mock.Of<Discount>();

        DiscountedChampion discountedChampion = new(discount, champion, oldPrice, newPrice);
        
        Assert.That(discountedChampion.Discount, Is.EqualTo(discount));
        Assert.That(discountedChampion.ChampionId, Is.EqualTo(championId));
        Assert.That(discountedChampion.OldPrice, Is.EqualTo(oldPrice));
        Assert.That(discountedChampion.NewPrice, Is.EqualTo(newPrice));
    }
}