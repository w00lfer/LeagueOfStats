using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Domain.Skins;
using Moq;
using NodaTime;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Discounts;

[TestFixture]
public class DiscountDomainServiceTests
{
    private readonly Mock<IDiscountRepository> _discountRepositoryMock = new();

    private DiscountDomainService _discountDomainService;

    [SetUp]
    public void SetUp()
    {
        _discountDomainService = new DiscountDomainService(_discountRepositoryMock.Object);
    }
    
    [Test]
    public async Task GetByIdAsync_DiscountDoesNotExist_ReturnsEntityNotFoundError()
    {
        Guid invalidDiscountId = Guid.NewGuid();
        _discountRepositoryMock
            .Setup(x => x.GetByIdAsync(invalidDiscountId))
            .ReturnsAsync((Discount)null);

        Result<Discount> result = await _discountDomainService.GetByIdAsync(invalidDiscountId);
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<EntityNotFoundError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo($"Discount with Id={invalidDiscountId} does not exist."));
        
        _discountRepositoryMock.Verify(x => x.GetByIdAsync(invalidDiscountId), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_DiscountExists_ReturnsDiscount()
    {
        Guid discountId = Guid.NewGuid();
        Discount discount = Mock.Of<Discount>(d => d.Id == discountId);
        _discountRepositoryMock
            .Setup(x => x.GetByIdAsync(discountId))
            .ReturnsAsync(discount);

        Result<Discount> result = await _discountDomainService.GetByIdAsync(discountId);
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(discount));
        
        _discountRepositoryMock.Verify(x => x.GetByIdAsync(discountId), Times.Once);
    }

    [Test]
    public async Task GetAllAsync_AllValid_ReturnsAllDiscounts()
    {
        Discount discount = Mock.Of<Discount>();
        IEnumerable<Discount> expectedDiscounts = new List<Discount>()
        {
            discount
        };
        _discountRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(expectedDiscounts);

        IEnumerable<Discount> discounts = await _discountDomainService.GetAllAsync();
        
        Assert.That(discounts, Is.EqualTo(expectedDiscounts));
        
        _discountRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task AddAsync_AllValid_CreatesDiscountAndCallsRepoAddAsyncAndReturnDiscount()
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

        Discount discount = await _discountDomainService.AddAsync(addDiscountDto);
        
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
        
        _discountRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Discount>()), Times.Once);
    }
}