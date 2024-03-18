using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners.Dtos;
using Moq;
using NodaTime;
using NUnit.Framework;

namespace LeagueOfStats.Domain.Tests.Summoners;

[TestFixture]
public class SummonerDomainServiceTests
{
    private readonly Mock<ISummonerRepository> _summonerRepositoryMock = new();
    private readonly Mock<IClock> _clockMock = new();

    private SummonerDomainService _summonerDomainService;

    [SetUp]
    public void SetUp()
    {
        _summonerRepositoryMock.Reset();
        _clockMock.Reset();
        
        _summonerDomainService = new SummonerDomainService(_summonerRepositoryMock.Object, _clockMock.Object);
    }
    
    [Test]
    public async Task GetByIdAsync_SummonerDoesNotExist_ReturnsEntityNotFoundError()
    {
        Guid invalidSummonerId = Guid.NewGuid();
        _summonerRepositoryMock
            .Setup(x => x.GetByIdWithAllIncludesAsync(invalidSummonerId))
            .ReturnsAsync((Summoner)null);

        Result<Summoner> result = await _summonerDomainService.GetByIdAsync(invalidSummonerId);
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<EntityNotFoundError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo($"Summoner with Id={invalidSummonerId} does not exist."));
        
        _summonerRepositoryMock.Verify(x => x.GetByIdWithAllIncludesAsync(invalidSummonerId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetByIdAsync_SummonerExists_ReturnsSummoner()
    {
        Guid summonerId = Guid.NewGuid();
        Summoner summoner = Mock.Of<Summoner>(d => d.Id == summonerId);
        _summonerRepositoryMock
            .Setup(x => x.GetByIdWithAllIncludesAsync(summonerId))
            .ReturnsAsync(summoner);

        Result<Summoner> result = await _summonerDomainService.GetByIdAsync(summonerId);
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(summoner));
        
        _summonerRepositoryMock.Verify(x => x.GetByIdWithAllIncludesAsync(summonerId), Times.Once); 
        VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetByPuuid_SummonerWithGivenPuuidDoesNotExist_ReturnsEntityNotFoundError()
    {
        const string invalidPuuid = "invalidPuuid";
        _summonerRepositoryMock
            .Setup(x => x.GetByPuuidAsync(invalidPuuid))
            .ReturnsAsync((Summoner)null);

        Result<Summoner> result = await _summonerDomainService.GetByPuuidAsync(invalidPuuid);
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<EntityNotFoundError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo($"Summoner with Puuid={invalidPuuid} does not exist."));
        
        _summonerRepositoryMock.Verify(x => x.GetByPuuidAsync(invalidPuuid), Times.Once);
        VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetByPuuid_SummonerExists_ReturnsSummoner()
    {
        const string puuid = "invalidPuuid";
        Summoner summoner = Mock.Of<Summoner>();
        _summonerRepositoryMock
            .Setup(x => x.GetByPuuidAsync(puuid))
            .ReturnsAsync(summoner);

        Result<Summoner> result = await _summonerDomainService.GetByPuuidAsync(puuid);
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(summoner));
        
        _summonerRepositoryMock.Verify(x => x.GetByPuuidAsync(puuid), Times.Once); 
        VerifyNoOtherCalls();
    }

    [Test]
    public async Task CreateAsync_AllValid_CreatesSummonerAndCallsAddAsyncRepoAndReturnsSummoner()
    {
        const string summonerId = "summonerId";
        const string accountId = "accountId";
        const string name = "name";
        const int profileIconId = 1000;
        const string puuid = "puuid";
        const long summonerLevel = 500;
        const string gameName = "gameName";
        const string tagLine = "tagLine";
        const Region region = Region.EUNE;

        Guid championId = Guid.NewGuid();
        Champion champion = Mock.Of<Champion>(c => c.Id == championId);
        const int championLevel = 5;
        const int championPoints = 100000;
        const long championPointsSinceLastLevel = 10000;
        const long championPointsUntilNextLevel = 0;
        const bool chestGranted = true;
        const long lastPlayTime = 10;
        const int tokensEarned = 2;
        UpdateChampionMasteryDto updateChampionMasteryDto = new(
            champion,
            championLevel,
            championPoints,
            championPointsSinceLastLevel,
            championPointsUntilNextLevel,
            chestGranted, lastPlayTime,
            tokensEarned);
        List<UpdateChampionMasteryDto> updateChampionMasteryDtos = new()
        {
            updateChampionMasteryDto
        };

        CreateSummonerDto createSummonerDto = new(
            summonerId,
            accountId,
            name,
            profileIconId,
            puuid,
            summonerLevel,
            gameName,
            tagLine,
            region,
            updateChampionMasteryDtos);
        
        Instant currentInstant = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(currentInstant);

        Summoner summoner = await _summonerDomainService.CreateAsync(createSummonerDto);
        
        Assert.That(summoner.SummonerId, Is.EqualTo(summonerId));
        Assert.That(summoner.AccountId, Is.EqualTo(accountId));
        Assert.That(summoner.SummonerName, Is.EqualTo(SummonerName.Create(gameName, tagLine)));
        Assert.That(summoner.Name, Is.EqualTo(name));
        Assert.That(summoner.ProfileIconId, Is.EqualTo(profileIconId));
        Assert.That(summoner.Puuid, Is.EqualTo(puuid));
        Assert.That(summoner.SummonerLevel, Is.EqualTo(summonerLevel));
        Assert.That(summoner.Region, Is.EqualTo(region));
        Assert.That(summoner.LastUpdated, Is.EqualTo(currentInstant));
        
        Assert.That(summoner.SummonerChampionMasteries.Count, Is.EqualTo(1));
        SummonerChampionMastery summonerChampionMastery = summoner.SummonerChampionMasteries.Single();
        
        Assert.That(summonerChampionMastery.Summoner, Is.EqualTo(summoner));
        Assert.That(summonerChampionMastery.ChampionId, Is.EqualTo(championId));
        Assert.That(summonerChampionMastery.ChampionLevel, Is.EqualTo(championLevel));
        Assert.That(summonerChampionMastery.ChampionPoints, Is.EqualTo(championPoints));
        Assert.That(summonerChampionMastery.ChampionPointsSinceLastLevel, Is.EqualTo(championPointsSinceLastLevel));
        Assert.That(summonerChampionMastery.ChampionPointsUntilNextLevel, Is.EqualTo(championPointsUntilNextLevel));
        Assert.That(summonerChampionMastery.ChestGranted, Is.EqualTo(chestGranted));
        Assert.That(summonerChampionMastery.LastPlayTime, Is.EqualTo(lastPlayTime));
        Assert.That(summonerChampionMastery.TokensEarned, Is.EqualTo(tokensEarned));
        
        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Once);
        _summonerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Summoner>()), Times.Once);
        VerifyNoOtherCalls();
    }

    [Test]
    public async Task CreateMultipleAsync_AllValid_CreatesMultipleSummonersAndCallsAddRangeAsyncRepoAndReturnsSummoners()
    {
        const string summonerId1 = "summonerId1";
        const string summonerId2 = "summonerId2";
        const string accountId1 = "accountId1";
        const string accountId2 = "accountId2";
        const string name1 = "name1";
        const string name2 = "name2";
        const int profileIconId1 = 1000;
        const int profileIconId2 = 2000;
        const string puuid1 = "puuid1";
        const string puuid2 = "puuid2";
        const long summonerLevel1 = 500;
        const long summonerLevel2 = 600;
        const string gameName1 = "gameName1";
        const string gameName2 = "gameName2";
        const string tagLine1 = "tagLine1";
        const string tagLine2 = "tagLine2";
        const Region region1 = Region.EUNE;
        const Region region2 = Region.EUW;
        
        CreateSummonerDto createSummonerDto1 = new(
            summonerId1,
            accountId1,
            name1,
            profileIconId1,
            puuid1,
            summonerLevel1,
            gameName1,
            tagLine1,
            region1,
            Enumerable.Empty<UpdateChampionMasteryDto>());
        CreateSummonerDto createSummonerDto2 = new(
            summonerId2,
            accountId2,
            name2,
            profileIconId2,
            puuid2,
            summonerLevel2,
            gameName2,
            tagLine2,
            region2,
            Enumerable.Empty<UpdateChampionMasteryDto>());

        IEnumerable<CreateSummonerDto> createSummonerDtos = new List<CreateSummonerDto>
        {
            createSummonerDto1,
            createSummonerDto2
        };
        
        Instant currentInstant = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(currentInstant);

        IEnumerable<Summoner> summoners = await _summonerDomainService.CreateMultipleAsync(createSummonerDtos);

        Assert.That(summoners.Count(), Is.EqualTo(2));

        Summoner summoner = summoners.ElementAt(0);
        Assert.That(summoner.SummonerId, Is.EqualTo(summonerId1));
        Assert.That(summoner.AccountId, Is.EqualTo(accountId1));
        Assert.That(summoner.SummonerName, Is.EqualTo(SummonerName.Create(gameName1, tagLine1)));
        Assert.That(summoner.Name, Is.EqualTo(name1));
        Assert.That(summoner.ProfileIconId, Is.EqualTo(profileIconId1));
        Assert.That(summoner.Puuid, Is.EqualTo(puuid1));
        Assert.That(summoner.SummonerLevel, Is.EqualTo(summonerLevel1));
        Assert.That(summoner.Region, Is.EqualTo(region1));
        Assert.That(summoner.LastUpdated, Is.EqualTo(currentInstant));
        Assert.That(summoner.SummonerChampionMasteries.Count, Is.EqualTo(0));
        
        summoner = summoners.ElementAt(1);
        Assert.That(summoner.SummonerId, Is.EqualTo(summonerId2));
        Assert.That(summoner.AccountId, Is.EqualTo(accountId2));
        Assert.That(summoner.SummonerName, Is.EqualTo(SummonerName.Create(gameName2, tagLine2)));
        Assert.That(summoner.Name, Is.EqualTo(name2));
        Assert.That(summoner.ProfileIconId, Is.EqualTo(profileIconId2));
        Assert.That(summoner.Puuid, Is.EqualTo(puuid2));
        Assert.That(summoner.SummonerLevel, Is.EqualTo(summonerLevel2));
        Assert.That(summoner.Region, Is.EqualTo(region2));
        Assert.That(summoner.LastUpdated, Is.EqualTo(currentInstant));
        Assert.That(summoner.SummonerChampionMasteries.Count, Is.EqualTo(0));
        
        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Exactly(2));
        _summonerRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<Summoner>>()), Times.Once);
        VerifyNoOtherCalls();
    }

    [Test]
    public async Task UpdateDetailsAsync_AllValid_CallsRepoUpdateAsyncAndUpdatesSummonerWithGivenData()
    {
        Guid championId1 = Guid.NewGuid();
        Champion champion1 = Mock.Of<Champion>(c => c.Id == championId1);
        UpdateChampionMasteryDto updateChampionMasteryDto = new(
            champion1,
            5,
            50000,
            10000,
            0,
            false,
            10,
            1);
        List<UpdateChampionMasteryDto> updateChampionMasteryDtos = new()
        {
            updateChampionMasteryDto
        };

        Summoner summoner = new Summoner(
            "summonerId",
            "accountId",
            "name",
            25,
            "puuid",
            500,
            "gameName",
            "tagLine",
            Region.EUNE,
            updateChampionMasteryDtos,
            Instant.MinValue);
        
        const int newChampionLevel1 = 5;
        const int newChampionPoints1 = 100000;
        const long newChampionPointsSinceLastLevel1 = 10000;
        const long newChampionPointsUntilNextLevel1 = 0;
        const bool newChestGranted1 = true;
        const long newLastPlayTime1 = 10;
        const int newTokensEarned1 = 2;
        UpdateChampionMasteryDto updateChampionMasteryDtoForExistingChampionMastery = new(
            champion1,
            newChampionLevel1,
            newChampionPoints1,
            newChampionPointsSinceLastLevel1,
            newChampionPointsUntilNextLevel1,
            newChestGranted1,
            newLastPlayTime1,
            newTokensEarned1);
        
        Guid championId2 = Guid.NewGuid();
        Champion champion2 = Mock.Of<Champion>(c => c.Id == championId2);
        const int newChampionLevel2 = 5;
        const int newChampionPoints2 = 100000;
        const long newChampionPointsSinceLastLevel2 = 10000;
        const long newChampionPointsUntilNextLevel2 = 0;
        const bool newChestGranted2 = true;
        const long newLastPlayTime2 = 10;
        const int newTokensEarned2 = 2;
        UpdateChampionMasteryDto updateChampionMasteryDtoForNewChampionMastery = new(
            champion2,
            newChampionLevel2,
            newChampionPoints2,
            newChampionPointsSinceLastLevel2,
            newChampionPointsUntilNextLevel2,
            newChestGranted2,
            newLastPlayTime2,
            newTokensEarned2);
        List<UpdateChampionMasteryDto> updateChampionMasteryDtosForUpdate = new()
        {
            updateChampionMasteryDtoForNewChampionMastery,
            updateChampionMasteryDtoForExistingChampionMastery
        };
        
        const int newProfileIconId = 100;
        const long newSummonerLevel = 600;
        
        UpdateDetailsSummonerDto updateDetailsSummonerDto = new(
            newProfileIconId,
            newSummonerLevel,
            updateChampionMasteryDtosForUpdate);
        
        Instant newLastUpdated = Instant.MaxValue;
        _clockMock
            .Setup(x => x.GetCurrentInstant())
            .Returns(newLastUpdated);

        await _summonerDomainService.UpdateDetailsAsync(summoner, updateDetailsSummonerDto);
        
        Assert.That(summoner.ProfileIconId, Is.EqualTo(newProfileIconId));
        Assert.That(summoner.SummonerLevel, Is.EqualTo(newSummonerLevel));
        Assert.That(summoner.LastUpdated, Is.EqualTo(newLastUpdated));
        
        Assert.That(summoner.SummonerChampionMasteries.Count, Is.EqualTo(2));
        SummonerChampionMastery summonerChampionMastery = summoner.SummonerChampionMasteries.ElementAt(0);
        
        Assert.That(summonerChampionMastery.Summoner, Is.EqualTo(summoner));
        Assert.That(summonerChampionMastery.ChampionId, Is.EqualTo(championId1));
        Assert.That(summonerChampionMastery.ChampionLevel, Is.EqualTo(newChampionLevel1));
        Assert.That(summonerChampionMastery.ChampionPoints, Is.EqualTo(newChampionPoints1));
        Assert.That(summonerChampionMastery.ChampionPointsSinceLastLevel, Is.EqualTo(newChampionPointsSinceLastLevel1));
        Assert.That(summonerChampionMastery.ChampionPointsUntilNextLevel, Is.EqualTo(newChampionPointsUntilNextLevel1));
        Assert.That(summonerChampionMastery.ChestGranted, Is.EqualTo(newChestGranted1));
        Assert.That(summonerChampionMastery.LastPlayTime, Is.EqualTo(newLastPlayTime1));
        Assert.That(summonerChampionMastery.TokensEarned, Is.EqualTo(newTokensEarned1));
        
        summonerChampionMastery = summoner.SummonerChampionMasteries.ElementAt(1);
        
        Assert.That(summonerChampionMastery.Summoner, Is.EqualTo(summoner));
        Assert.That(summonerChampionMastery.ChampionId, Is.EqualTo(championId2));
        Assert.That(summonerChampionMastery.ChampionLevel, Is.EqualTo(newChampionLevel2));
        Assert.That(summonerChampionMastery.ChampionPoints, Is.EqualTo(newChampionPoints2));
        Assert.That(summonerChampionMastery.ChampionPointsSinceLastLevel, Is.EqualTo(newChampionPointsSinceLastLevel2));
        Assert.That(summonerChampionMastery.ChampionPointsUntilNextLevel, Is.EqualTo(newChampionPointsUntilNextLevel2));
        Assert.That(summonerChampionMastery.ChestGranted, Is.EqualTo(newChestGranted2));
        Assert.That(summonerChampionMastery.LastPlayTime, Is.EqualTo(newLastPlayTime2));
        Assert.That(summonerChampionMastery.TokensEarned, Is.EqualTo(newTokensEarned2));

        _clockMock.Verify(x => x.GetCurrentInstant(), Times.Once);
        _summonerRepositoryMock.Verify(x => x.UpdateAsync(summoner), Times.Once);
        VerifyNoOtherCalls();
    }

    private void VerifyNoOtherCalls()
    {
        _summonerRepositoryMock.VerifyNoOtherCalls();
        _clockMock.VerifyNoOtherCalls();
    }
}