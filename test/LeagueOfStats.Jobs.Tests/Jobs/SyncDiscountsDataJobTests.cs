using System.Data;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Repositories;
using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient;
using LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient.Enums;
using LeagueOfStats.Jobs.Jobs;
using Moq;
using NodaTime;
using NUnit.Framework;
using Quartz;
using Champion = LeagueOfStats.Domain.Champions.Champion;

namespace LeagueOfStats.Jobs.Tests.Jobs;

[TestFixture]
public class SyncDiscountsDataJobTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IRiotGamesShopClient> _riotGamesShopClientMock = new();
    private readonly Mock<IDiscountRepository> _discountRepositoryMock = new();
    private readonly Mock<IChampionRepository> _championRepositoryMock = new();
    private readonly Mock<ISkinRepository> _skinRepositoryMock = new();
    private readonly Mock<IClock> _clockMock = new();

    private SyncDiscountsDataJob _syncDiscountsDataJob;
    
    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock.Reset();
        _riotGamesShopClientMock.Reset();
        _discountRepositoryMock.Reset();
        _championRepositoryMock.Reset();
        _skinRepositoryMock.Reset();
        _clockMock.Reset();

        _syncDiscountsDataJob = new(
            _unitOfWorkMock.Object,
            _riotGamesShopClientMock.Object,
            _discountRepositoryMock.Object,
            _championRepositoryMock.Object,
            _skinRepositoryMock.Object,
            _clockMock.Object);
    }
    
    [Test]
    public async Task Execute_CurrentDiscountIsAlreadyPersisted_DoesNothingAndCoommitTransaction()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);

        Instant currentInstant = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(currentInstant);
        
        LocalDateTime currentDateTime = currentInstant.InUtc().LocalDateTime;
        _discountRepositoryMock
            .Setup(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime))
            .ReturnsAsync(true);
        
        await _syncDiscountsDataJob.Execute(Mock.Of<IJobExecutionContext>());
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Once);
        _discountRepositoryMock.Verify(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime), Times.Once);
        dbTransactionMock.Verify(x => x.Commit(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Execute_RiotGamesShopClientReturnsError_DoesNothingAndRollbackTransaction()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);
        
        Instant currentInstant = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(currentInstant);
        
        LocalDateTime currentDateTime = currentInstant.InUtc().LocalDateTime;
        _discountRepositoryMock
            .Setup(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime))
            .ReturnsAsync(false);
        
        _riotGamesShopClientMock
            .Setup(x => x.GetCurrentDiscountsAsync())
            .ReturnsAsync(new EntityNotFoundError("error"));
        
        await _syncDiscountsDataJob.Execute(Mock.Of<IJobExecutionContext>());
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Once);
        _discountRepositoryMock.Verify(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime), Times.Once);
        _riotGamesShopClientMock.Verify(x => x.GetCurrentDiscountsAsync(), Times.Once);
        dbTransactionMock.Verify(x => x.Rollback(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Execute_RiotGamesShopClientReturnsEmptyListOfDiscounts_DoesNothingAndRollbackTransaction()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);

        Instant currentInstant = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(currentInstant);
        
        LocalDateTime currentDateTime = currentInstant.InUtc().LocalDateTime;
        _discountRepositoryMock
            .Setup(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime))
            .ReturnsAsync(false);
        
        _riotGamesShopClientMock
            .Setup(x => x.GetCurrentDiscountsAsync())
            .ReturnsAsync(Result.Success(Enumerable.Empty<RiotGamesShopDiscount>()));
        
        await _syncDiscountsDataJob.Execute(Mock.Of<IJobExecutionContext>());
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Once);
        _discountRepositoryMock.Verify(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime), Times.Once);
        _riotGamesShopClientMock.Verify(x => x.GetCurrentDiscountsAsync(), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default));
        dbTransactionMock.Verify(x => x.Commit(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Execute_OneOfDiscountsHasDifferentStartDateThanOthers_DoesNothingAndRollbackTransaction()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);

        Instant currentInstant = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(currentInstant);
        
        LocalDateTime currentDateTime = currentInstant.InUtc().LocalDateTime;
        _discountRepositoryMock
            .Setup(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime))
            .ReturnsAsync(false);

        RiotGamesShopDiscount riotGamesShopDiscount1 = new(1, DiscountType.Champion, 10, 5, new LocalDateTime(2024, 3, 17, 12, 0), LocalDateTime.MaxIsoValue);
        RiotGamesShopDiscount riotGamesShopDiscount2 = new(2, DiscountType.Champion, 10, 5, new LocalDateTime(2024, 3, 18, 12, 0), LocalDateTime.MaxIsoValue);
        IEnumerable<RiotGamesShopDiscount> riotGamesShopDiscounts = new List<RiotGamesShopDiscount>
        {
            riotGamesShopDiscount1,
            riotGamesShopDiscount2
        };
        _riotGamesShopClientMock
            .Setup(x => x.GetCurrentDiscountsAsync())
            .ReturnsAsync(Result.Success(riotGamesShopDiscounts));
        
        await _syncDiscountsDataJob.Execute(Mock.Of<IJobExecutionContext>());
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Once);
        _discountRepositoryMock.Verify(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime), Times.Once);
        _riotGamesShopClientMock.Verify(x => x.GetCurrentDiscountsAsync(), Times.Once);
        dbTransactionMock.Verify(x => x.Rollback(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Execute_OneOfDiscountsHasDifferentEndDateThanOthers_DoesNothingAndRollbackTransaction()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);

        Instant currentInstant = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(currentInstant);
        
        LocalDateTime currentDateTime = currentInstant.InUtc().LocalDateTime;
        _discountRepositoryMock
            .Setup(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime))
            .ReturnsAsync(false);

        RiotGamesShopDiscount riotGamesShopDiscount1 = new(1, DiscountType.Champion, 10, 5, LocalDateTime.MaxIsoValue, new LocalDateTime(2024, 3, 17, 12, 0));
        RiotGamesShopDiscount riotGamesShopDiscount2 = new(2, DiscountType.Champion, 10, 5, LocalDateTime.MaxIsoValue , new LocalDateTime(2024, 3, 18, 12, 0));
        IEnumerable<RiotGamesShopDiscount> riotGamesShopDiscounts = new List<RiotGamesShopDiscount>
        {
            riotGamesShopDiscount1,
            riotGamesShopDiscount2
        };
        _riotGamesShopClientMock
            .Setup(x => x.GetCurrentDiscountsAsync())
            .ReturnsAsync(Result.Success(riotGamesShopDiscounts));
        
        await _syncDiscountsDataJob.Execute(Mock.Of<IJobExecutionContext>());
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Once);
        _discountRepositoryMock.Verify(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime), Times.Once);
        _riotGamesShopClientMock.Verify(x => x.GetCurrentDiscountsAsync(), Times.Once);
        dbTransactionMock.Verify(x => x.Rollback(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Execute_OneOfChampionRiotIdFromDiscountsIsNotPresentInPersistedChampions_DoesNothingAndRollbackTransaction()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);

        Instant currentInstant = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(currentInstant);
        
        LocalDateTime currentDateTime = currentInstant.InUtc().LocalDateTime;
        _discountRepositoryMock
            .Setup(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime))
            .ReturnsAsync(false);

        RiotGamesShopDiscount riotGamesShopDiscount1 = new(1, DiscountType.Champion, 10, 5, LocalDateTime.MinIsoValue, LocalDateTime.MaxIsoValue);
        RiotGamesShopDiscount riotGamesShopDiscount2 = new(2, DiscountType.Champion, 10, 5, LocalDateTime.MinIsoValue , LocalDateTime.MaxIsoValue);
        IEnumerable<RiotGamesShopDiscount> riotGamesShopDiscounts = new List<RiotGamesShopDiscount>
        {
            riotGamesShopDiscount1,
            riotGamesShopDiscount2
        };
        _riotGamesShopClientMock
            .Setup(x => x.GetCurrentDiscountsAsync())
            .ReturnsAsync(Result.Success(riotGamesShopDiscounts));
        
        Champion champion = new Champion(1, "name", "title", "description", "splashUrl", "uncenteredSplashUrl", "iconUrl", "tileUrl");
        IEnumerable<Champion> champions = new List<Champion>
        {
            champion
        };
        _championRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(champions);
        
        await _syncDiscountsDataJob.Execute(Mock.Of<IJobExecutionContext>());
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Once);
        _discountRepositoryMock.Verify(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime), Times.Once);
        _riotGamesShopClientMock.Verify(x => x.GetCurrentDiscountsAsync(), Times.Once);
        _championRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        dbTransactionMock.Verify(x => x.Rollback(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Execute_OneOfSkinsRiotIdFromDiscountsIsNotPresentInPersistedSkins_DoesNothingAndRollbackTransaction()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);

        Instant currentInstant = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(currentInstant);
        
        LocalDateTime currentDateTime = currentInstant.InUtc().LocalDateTime;
        _discountRepositoryMock
            .Setup(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime))
            .ReturnsAsync(false);

        RiotGamesShopDiscount riotGamesShopDiscount1 = new(1, DiscountType.ChampionSkin, 10, 5, LocalDateTime.MinIsoValue, LocalDateTime.MaxIsoValue);
        RiotGamesShopDiscount riotGamesShopDiscount2 = new(2, DiscountType.ChampionSkin, 10, 5, LocalDateTime.MinIsoValue , LocalDateTime.MaxIsoValue);
        IEnumerable<RiotGamesShopDiscount> riotGamesShopDiscounts = new List<RiotGamesShopDiscount>
        {
            riotGamesShopDiscount1,
            riotGamesShopDiscount2
        };
        _riotGamesShopClientMock
            .Setup(x => x.GetCurrentDiscountsAsync())
            .ReturnsAsync(Result.Success(riotGamesShopDiscounts));
        
        Champion champion1 = new Champion(1, "name", "title", "description", "splashUrl", "uncenteredSplashUrl", "iconUrl", "tileUrl");
        Champion champion2 = new Champion(2, "name", "title", "description", "splashUrl", "uncenteredSplashUrl", "iconUrl", "tileUrl");
        IEnumerable<Champion> champions = new List<Champion>
        {
            champion1,
            champion2
        };
        _championRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(champions);

        Skin skin = new Skin(new AddSkinDto(3, false, "name", "description", "splashUrl", "uncenteredSplashUrl", "tileUrl", "rarity", false, "chromaPath", Enumerable.Empty<AddSkinChromaDto>()));
        IEnumerable<Skin> skins = new List<Skin>
        {
            skin
        };
        _skinRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(skins);
        
        await _syncDiscountsDataJob.Execute(Mock.Of<IJobExecutionContext>());
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Once);
        _discountRepositoryMock.Verify(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime), Times.Once);
        _riotGamesShopClientMock.Verify(x => x.GetCurrentDiscountsAsync(), Times.Once);
        _championRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        _skinRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        dbTransactionMock.Verify(x => x.Rollback(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        VerifyNoOtherCalls();
    }
    
         [Test]
    public async Task Execute_AllValidAndCurrentDiscountIsNotYetPersisted_AddNewDiscountAndCommitsTransaction()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);

        Instant currentInstant = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(currentInstant);
        
        LocalDateTime currentDateTime = currentInstant.InUtc().LocalDateTime;
        _discountRepositoryMock
            .Setup(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime))
            .ReturnsAsync(false);

        RiotGamesShopDiscount riotGamesShopDiscount1 = new(1, DiscountType.Champion, 10, 5, LocalDateTime.MinIsoValue, LocalDateTime.MaxIsoValue);
        RiotGamesShopDiscount riotGamesShopDiscount2 = new(2, DiscountType.Champion, 10, 5, LocalDateTime.MinIsoValue , LocalDateTime.MaxIsoValue);
        RiotGamesShopDiscount riotGamesShopDiscount3 = new(3, DiscountType.ChampionSkin, 10, 5, LocalDateTime.MinIsoValue, LocalDateTime.MaxIsoValue);
        RiotGamesShopDiscount riotGamesShopDiscount4 = new(4, DiscountType.ChampionSkin, 10, 5, LocalDateTime.MinIsoValue , LocalDateTime.MaxIsoValue);
        IEnumerable<RiotGamesShopDiscount> riotGamesShopDiscounts = new List<RiotGamesShopDiscount>
        {
            riotGamesShopDiscount1,
            riotGamesShopDiscount2,
            riotGamesShopDiscount3,
            riotGamesShopDiscount4
        };
        _riotGamesShopClientMock
            .Setup(x => x.GetCurrentDiscountsAsync())
            .ReturnsAsync(Result.Success(riotGamesShopDiscounts));
        
        Champion champion1 = new Champion(1, "name", "title", "description", "splashUrl", "uncenteredSplashUrl", "iconUrl", "tileUrl");
        Champion champion2 = new Champion(2, "name", "title", "description", "splashUrl", "uncenteredSplashUrl", "iconUrl", "tileUrl");
        IEnumerable<Champion> champions = new List<Champion>
        {
            champion1,
            champion2
        };
        _championRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(champions);

        Skin skin1 = new Skin(new AddSkinDto(3, false, "name", "description", "splashUrl", "uncenteredSplashUrl", "tileUrl", "rarity", false, "chromaPath", Enumerable.Empty<AddSkinChromaDto>()));
        Skin skin2 = new Skin(new AddSkinDto(4, false, "name", "description", "splashUrl", "uncenteredSplashUrl", "tileUrl", "rarity", false, "chromaPath", Enumerable.Empty<AddSkinChromaDto>()));

        IEnumerable<Skin> skins = new List<Skin>
        {
            skin1,
            skin2
        };
        _skinRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(skins);
        
        await _syncDiscountsDataJob.Execute(Mock.Of<IJobExecutionContext>());
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Once);
        _discountRepositoryMock.Verify(x => x.DoesDiscountInGivenLocalDateTimeExistAsync(currentDateTime), Times.Once);
        _riotGamesShopClientMock.Verify(x => x.GetCurrentDiscountsAsync(), Times.Once);
        _championRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        _skinRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        _discountRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Discount>()));
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default));
        dbTransactionMock.Verify(x => x.Commit(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        VerifyNoOtherCalls();
    }
    
    private void VerifyNoOtherCalls()
    {
        _unitOfWorkMock.VerifyNoOtherCalls();
        _riotGamesShopClientMock.VerifyNoOtherCalls();
        _discountRepositoryMock.VerifyNoOtherCalls();
        _championRepositoryMock.VerifyNoOtherCalls();
        _skinRepositoryMock.VerifyNoOtherCalls();
        _clockMock.VerifyNoOtherCalls();
    }
}