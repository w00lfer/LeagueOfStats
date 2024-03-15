using System.Data;
using System.Data.Common;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Repositories;
using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Jobs.ApiClients.CommunityDragonClient;
using LeagueOfStats.Jobs.ApiClients.DataDragonClient;
using LeagueOfStats.Jobs.Jobs;
using Moq;
using NUnit.Framework;
using Quartz;

namespace LeagueOfStats.Jobs.Tests.Jobs;

[TestFixture]
public class SyncChampionAndSkinDataJobTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IChampionRepository> _championRepositoryMock = new();
    private readonly Mock<ISkinRepository> _skinRepositoryMock = new();
    private readonly Mock<IDataDragonClient> _dataDragonClientMock = new();
    private readonly Mock<ICommunityDragonClient> _communityDragonClientMock = new();

    private SyncChampionAndSkinDataJob _syncChampionAndSkinDataJob;
    
    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock.Reset();
        _championRepositoryMock.Reset();
        _skinRepositoryMock.Reset();
        _dataDragonClientMock.Reset();
        _communityDragonClientMock.Reset();
        
        _syncChampionAndSkinDataJob = new(
            _unitOfWorkMock.Object,
            _championRepositoryMock.Object,
            _skinRepositoryMock.Object,
            _dataDragonClientMock.Object,
            _communityDragonClientMock.Object);
    }

    [Test]
    public async Task Execute_DataDragonReturnsError_DoesNothingAndRollbackTransaction()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);
        
        _dataDragonClientMock
            .Setup(x => x.GetChampionsAsync())
            .ReturnsAsync(new EntityNotFoundError("error"));
        
        await _syncChampionAndSkinDataJob.Execute(Mock.Of<IJobExecutionContext>());
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        dbTransactionMock.Verify(x => x.Rollback(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        _dataDragonClientMock.Verify(x => x.GetChampionsAsync(), Times.Once);
        VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Execute_AllValid_AddAnyNewChampionAndSkinDataAndCommitTransaction()
    {
        Mock<IDbTransaction> dbTransactionMock = new();
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction())
            .Returns(dbTransactionMock.Object);

        var championDto1 = new ChampionDto(1, "name1", "title1", "description1");
        var championDto2 = new ChampionDto(2, "name1", "title2", "description2");
        IEnumerable<ChampionDto> championDtos = new List<ChampionDto>()
        {
            championDto1,
            championDto2
        };
        _dataDragonClientMock
            .Setup(x => x.GetChampionsAsync())
            .ReturnsAsync(Domain.Common.Rails.Results.Result.Success(championDtos));

        var champion = Mock.Of<Champion>(c => c.RiotChampionId == championDto1.RiotChampionId);
        IEnumerable<Champion> champions = new List<Champion>
        {
            champion
        };
        _championRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(champions);
        
        var skinDto1 = new SkinDto(1111, true, "name1", "description2", "rarity", false, "chromaPath", Enumerable.Empty<SkinChromaDto>());
        var skinDto2 = new SkinDto(2222, true, "name1", "description2", "rarity", false, "chromaPath", Enumerable.Empty<SkinChromaDto>());
        IEnumerable<SkinDto> skinDtos = new List<SkinDto>()
        {
            skinDto1,
            skinDto2
        };
        _communityDragonClientMock
            .Setup(x => x.GetSkinsAsync())
            .ReturnsAsync(Domain.Common.Rails.Results.Result.Success(skinDtos));

        var skin = Mock.Of<Skin>(s => s.RiotSkinId == skinDto1.RiotSkinId);
        IEnumerable<Skin> skins = new List<Skin>
        {
            skin
        };
        _skinRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(skins);
        
        await _syncChampionAndSkinDataJob.Execute(Mock.Of<IJobExecutionContext>());
        
        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once);
        _dataDragonClientMock.Verify(x => x.GetChampionsAsync(), Times.Once);
        _championRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        _championRepositoryMock.Verify(x => x.AddRangeAsync(It.Is<IEnumerable<Champion>>(ec => ec.Any(c =>
            c.RiotChampionId == championDto1.RiotChampionId))), Times.Once);
        _communityDragonClientMock.Verify(x => x.GetSkinsAsync(), Times.Once);
        _skinRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        _skinRepositoryMock.Verify(x => x.AddRangeAsync(It.Is<IEnumerable<Skin>>(es => es.Any(s =>
            s.RiotSkinId == skinDto1.RiotSkinId))), Times.Once);
        dbTransactionMock.Verify(x => x.Commit(), Times.Once);
        dbTransactionMock.Verify(x => x.Dispose(), Times.Once);
        VerifyNoOtherCalls();
    }

    private void VerifyNoOtherCalls()
    {
        _unitOfWorkMock.VerifyNoOtherCalls();
        _championRepositoryMock.VerifyNoOtherCalls();
        _skinRepositoryMock.VerifyNoOtherCalls();
        _dataDragonClientMock.VerifyNoOtherCalls();
        _communityDragonClientMock.VerifyNoOtherCalls();
    }
}