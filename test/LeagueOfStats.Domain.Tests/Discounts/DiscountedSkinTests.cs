using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Domain.Skins;
using Moq;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Discounts;

[TestFixture]
public class DiscountedSkinTests
{
    [Test]
    public void Constructor_AllValid_CreatesDiscountedSkinWithProvidedData()
    {
        const int oldPrice = 10;
        const int newPrice = 5;
        Guid skinId = Guid.NewGuid();
        Skin skin = Mock.Of<Skin>(s => s.Id == skinId);
        Discount discount = Mock.Of<Discount>();

        DiscountedSkin discountedSkin = new(discount, skin, oldPrice, newPrice);
        
        Assert.That(discountedSkin.Discount, Is.EqualTo(discount));
        Assert.That(discountedSkin.SkinId, Is.EqualTo(skinId));
        Assert.That(discountedSkin.OldPrice, Is.EqualTo(oldPrice));
        Assert.That(discountedSkin.NewPrice, Is.EqualTo(newPrice));
    }
}