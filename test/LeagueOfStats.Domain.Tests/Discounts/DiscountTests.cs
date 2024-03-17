using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Domain.Skins;
using Moq;
using NodaTime;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Discounts;

[TestFixture]
public class DiscountTests
{
    [Test]
    public void Constructor_AllValid_CreateDiscountWithDiscountedChampionsAndDiscountedSkinsWithProvidedData()
    {
        LocalDateTime startDateTime = LocalDateTime.MaxIsoValue;
        LocalDateTime endDateTime = LocalDateTime.MinIsoValue;

        const int championOldPrice = 10;
        const int championNewPrice = 5;
        Guid championId = Guid.NewGuid();
        AddDiscountedChampionDto addDiscountedChampionDto = new(
            Mock.Of<Champion>(c => c.Id == championId),
            championOldPrice,
            championNewPrice);
        IEnumerable<AddDiscountedChampionDto> addDiscountedChampionDtos = new List<AddDiscountedChampionDto>
        {
            addDiscountedChampionDto
        };
        
        const int skinOldPrice = 20;
        const int skinNewPrice = 15;
        Guid skinId = Guid.NewGuid();
        AddDiscountedSkinDto addDiscountedSkinDto = new(
            Mock.Of<Skin>(s => s.Id == skinId),
            skinOldPrice,
            skinNewPrice);
        IEnumerable<AddDiscountedSkinDto> addDiscountedSkinDtos = new List<AddDiscountedSkinDto>
        {
            addDiscountedSkinDto
        };

        AddDiscountDto addDiscountDto = new AddDiscountDto(
            startDateTime,
            endDateTime,
            addDiscountedChampionDtos,
            addDiscountedSkinDtos);
        
        Discount discount = new Discount(addDiscountDto);
        
        Assert.That(discount.StartDateTime, Is.EqualTo(startDateTime));
        Assert.That(discount.EndDateTime, Is.EqualTo(endDateTime));
        
        Assert.That(discount.DiscountedChampions.Count, Is.EqualTo(1));

        DiscountedChampion discountedChampion = discount.DiscountedChampions.ElementAt(0);
        Assert.That(discountedChampion.ChampionId, Is.EqualTo(championId));
        Assert.That(discountedChampion.OldPrice, Is.EqualTo(championOldPrice));
        Assert.That(discountedChampion.NewPrice, Is.EqualTo(championNewPrice));
        
        Assert.That(discount.DiscountedSkins.Count, Is.EqualTo(1));

        DiscountedSkin discountedSkin = discount.DiscountedSkins.ElementAt(0);
        Assert.That(discountedSkin.SkinId, Is.EqualTo(skinId));
        Assert.That(discountedSkin.OldPrice, Is.EqualTo(skinOldPrice));
        Assert.That(discountedSkin.NewPrice, Is.EqualTo(skinNewPrice));
    }
}