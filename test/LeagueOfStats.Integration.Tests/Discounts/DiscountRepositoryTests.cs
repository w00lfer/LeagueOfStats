using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Infrastructure.Discounts;
using LeagueOfStats.Integration.Tests.TestCommons;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NUnit.Framework;

namespace LeagueOfStats.Integration.Tests.Discounts;

[TestFixture]
public class DiscountRepositoryTests : IntegrationTestBase
{
    private DiscountRepository _discountRepository;

    [SetUp]
    public void SetUp()
    {
        _discountRepository = new DiscountRepository(ApplicationDbContext);
    }

    [Test]
    public async Task GetByIdAsync_DiscountWithIdExists_ReturnsDiscount()
    {
        Discount discount = CreateDiscount();

        await ApplicationDbContext.AddAsync(discount);
        await ApplicationDbContext.SaveChangesAsync();

        Discount? discountFromRepo = await _discountRepository.GetByIdAsync(discount.Id);

        Assert.That(discountFromRepo, Is.Not.Null);
        Assert.That(discountFromRepo, Is.EqualTo(discount));
        Assert.That(ApplicationDbContext.Entry(discountFromRepo).Collection(d => d.DiscountedChampions).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo).Collection(d => d.DiscountedSkins).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetByIdAsync_DiscountWithIdDoesNotExist_ReturnsNull()
    {
        Discount? discountFromRepo = await _discountRepository.GetByIdAsync(Guid.NewGuid());

        Assert.That(discountFromRepo, Is.Null);
    }

    [Test]
    public async Task GetAllAsync_IdsAreEmpty_ReturnsAllDiscounts()
    {
        Discount discount1 = CreateDiscount();
        Discount discount2 = CreateDiscount();

        await ApplicationDbContext.AddRangeAsync(discount1, discount2);
        await ApplicationDbContext.SaveChangesAsync();

        List<Discount> discountFromRepo = (await _discountRepository.GetAllAsync()).ToList();

        Assert.That(discountFromRepo.Count(), Is.EqualTo(2));
        Assert.That(discountFromRepo.Contains(discount1), Is.True);
        Assert.That(discountFromRepo.Contains(discount2), Is.True);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(0)).Collection(d => d.DiscountedChampions).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(0)).Collection(d => d.DiscountedSkins).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(1)).Collection(d => d.DiscountedChampions).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(1)).Collection(d => d.DiscountedSkins).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndAllDiscountsWithGivenIdsExist_ReturnsAllDiscountsWithGivenIds()
    {
        Discount discount1 = CreateDiscount();
        Discount discount2 = CreateDiscount();
        Discount discount3 = CreateDiscount();

        await ApplicationDbContext.AddRangeAsync(discount1, discount2, discount3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] discountIds =
        {
            discount1.Id,
            discount3.Id
        };
        List<Discount> discountFromRepo = (await _discountRepository.GetAllAsync(discountIds)).ToList();

        Assert.That(discountFromRepo.Count(), Is.EqualTo(2));
        Assert.That(discountFromRepo.Contains(discount1), Is.True);
        Assert.That(discountFromRepo.Contains(discount3), Is.True);
        Assert.That(discountFromRepo.Contains(discount2), Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(0)).Collection(d => d.DiscountedChampions).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(0)).Collection(d => d.DiscountedSkins).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(1)).Collection(d => d.DiscountedChampions).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(1)).Collection(d => d.DiscountedSkins).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndSomeDiscountsWithGivenIdsExist_ReturnsSomeDiscountsWithGivenIds()
    {
        Discount discount1 = CreateDiscount();
        Discount discount2 = CreateDiscount();
        Discount discount3 = CreateDiscount();

        await ApplicationDbContext.AddRangeAsync(discount1, discount2, discount3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] discountIds =
        {
            discount1.Id,
            discount2.Id,
            Faker.Random.Guid()
        };
        List<Discount> discountFromRepo = (await _discountRepository.GetAllAsync(discountIds)).ToList();

        Assert.That(discountFromRepo.Count(), Is.EqualTo(2));
        Assert.That(discountFromRepo.Contains(discount1), Is.True);
        Assert.That(discountFromRepo.Contains(discount2), Is.True);
        Assert.That(discountFromRepo.Contains(discount3), Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(0)).Collection(d => d.DiscountedChampions).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(0)).Collection(d => d.DiscountedSkins).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(1)).Collection(d => d.DiscountedChampions).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(discountFromRepo.ElementAt(1)).Collection(d => d.DiscountedSkins).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndNoneDiscountsWithGivenIdsExist_ReturnsEmptyListOfDiscounts()
    {
        Discount discount1 = CreateDiscount();
        Discount discount2 = CreateDiscount();
        Discount discount3 = CreateDiscount();

        await ApplicationDbContext.AddRangeAsync(discount1, discount2, discount3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] discountIds =
        {
            Faker.Random.Guid(),
            Faker.Random.Guid()
        };
        List<Discount> discountFromRepo = (await _discountRepository.GetAllAsync(discountIds)).ToList();

        Assert.That(discountFromRepo, Is.Empty);
    }

    [Test]
    public async Task AddAsync_AllValid_AddsDiscount()
    {
        Discount discount = CreateDiscount();

        await _discountRepository.AddAsync(discount);

        IEnumerable<Discount> discountesInDb = await ApplicationDbContext.Discounts.ToListAsync();

        Assert.That(discountesInDb.Count(), Is.EqualTo(1));
        Assert.That(discountesInDb.Single(), Is.EqualTo(discount));
    }

    [Test]
    public async Task AddRangeAsync_AllValid_AddsDiscounts()
    {
        Discount discount1 = CreateDiscount();
        Discount discount2 = CreateDiscount();

        await _discountRepository.AddRangeAsync(new[] { discount1, discount2 });

        IEnumerable<Discount> discountesInDb = await ApplicationDbContext.Discounts.ToListAsync();

        Assert.That(discountesInDb.Count(), Is.EqualTo(2));
        Assert.That(discountesInDb.Contains(discount1), Is.True);
        Assert.That(discountesInDb.Contains(discount1), Is.True);
    }

    [Test]
    public async Task DeleteAsync_DiscountExists_DeletesDiscount()
    {
        Discount discount = CreateDiscount();

        await ApplicationDbContext.AddAsync(discount);
        await ApplicationDbContext.SaveChangesAsync();

        await _discountRepository.DeleteAsync(discount);

        Discount? discountFromDb = await ApplicationDbContext.Discounts.SingleOrDefaultAsync(m => m.Id == discount.Id);

        Assert.That(discountFromDb, Is.Null);
    }

    [Test]
    public async Task DoesDiscountInGivenLocalDateTimeExistAsync_DiscountExistWithinGivenLocalDateTime_ReturnsTrue()
    {
        LocalDateTime startDateTime = new LocalDateTime(2024, 3, 18, 12, 0);
        LocalDateTime endDateTime = new LocalDateTime(2024, 3, 25, 12, 0);
        LocalDateTime localDateTime = new LocalDateTime(2024, 3, 20, 12, 0);
        Discount discount = CreateDiscount(startDateTime, endDateTime);

        await ApplicationDbContext.AddAsync(discount);
        await ApplicationDbContext.SaveChangesAsync();

        bool doesDiscountInGivenLocalDateTimeExist = await _discountRepository.DoesDiscountInGivenLocalDateTimeExistAsync(localDateTime);
        
        Assert.That(doesDiscountInGivenLocalDateTimeExist, Is.True);
    }
    
    [Test]
    public async Task DoesDiscountInGivenLocalDateTimeExistAsync_DiscountDoesNotExistWithinGivenLocalDateTime_ReturnsFalse()
    {
        LocalDateTime startDateTime = new LocalDateTime(2024, 3, 18, 12, 0);
        LocalDateTime endDateTime = new LocalDateTime(2024, 3, 25, 12, 0);
        LocalDateTime localDateTime = new LocalDateTime(2024, 3, 26, 12, 0);
        Discount discount = CreateDiscount(startDateTime, endDateTime);

        await ApplicationDbContext.AddAsync(discount);
        await ApplicationDbContext.SaveChangesAsync();

        bool doesDiscountInGivenLocalDateTimeExist = await _discountRepository.DoesDiscountInGivenLocalDateTimeExistAsync(localDateTime);
        
        Assert.That(doesDiscountInGivenLocalDateTimeExist, Is.False);
    }

    private Discount CreateDiscount(LocalDateTime? startDateTimeOverride = null, LocalDateTime? endDateTimeOverride = null)
    {
        LocalDateTime startDateTime = startDateTimeOverride ?? LocalDateTime.FromDateTime(Faker.Date.Between(new DateTime(2000, 1, 1), new DateTime(2030, 1, 1)));
        LocalDateTime endDateTime = endDateTimeOverride ?? startDateTime.PlusDays(7);
        AddDiscountDto addDiscountDto = new(
            startDateTime,
            endDateTime,
            Enumerable.Empty<AddDiscountedChampionDto>(),
            Enumerable.Empty<AddDiscountedSkinDto>());

        Discount discount = new Discount(addDiscountDto);

        return discount;
    }
}