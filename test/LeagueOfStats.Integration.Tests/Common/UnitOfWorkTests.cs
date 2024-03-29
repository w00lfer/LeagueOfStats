using System.Data;
using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Infrastructure.Common;
using LeagueOfStats.Integration.Tests.TestCommons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using NodaTime;
using NUnit.Framework;

namespace LeagueOfStats.Integration.Tests.Common;

[TestFixture]
public class UnitOfWorkTests : IntegrationTestBase
{
    private UnitOfWork _unitOfWork;
    
    [SetUp]
    public void SetUp()
    {
        _unitOfWork = new(ApplicationDbContext);
    }

    [Test]
    public async Task SaveChangesAsync_AllValid_InvokesApplicationDbContextSaveChanges()
    {
        Discount discount = new Discount(new AddDiscountDto(
                new LocalDateTime(2024, 3, 22, 12, 0, 0),
                new LocalDateTime(2024, 3, 29, 12, 0, 0),
                Enumerable.Empty<AddDiscountedChampionDto>(),
                Enumerable.Empty<AddDiscountedSkinDto>()));

        await ApplicationDbContext.AddAsync(discount);
        
        await _unitOfWork.SaveChangesAsync();

        List<Discount> discountsFromDb = await ApplicationDbContext.Discounts.ToListAsync();
        
        Assert.That(discountsFromDb.Count, Is.EqualTo(1));
    }

    [Test]
    public void BeginTransaction_AllValid_BeginsTransactionAndReturnsTransaction()
    {
        IDbTransaction transaction = _unitOfWork.BeginTransaction();
        
        Assert.That(transaction, Is.EqualTo(ApplicationDbContext.Database.CurrentTransaction.GetDbTransaction()));
    }
}