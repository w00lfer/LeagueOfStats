using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Domain.Summoners.Dtos;
using LeagueOfStats.Infrastructure.Summoners;
using LeagueOfStats.Integration.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Moq;
using NodaTime;
using NUnit.Framework;

namespace LeagueOfStats.Integration.Tests.Summoners;

[TestFixture]
public class SummonerRepositoryTests : IntegrationTestBase
{
   private SummonerRepository _summonerRepository;

    [SetUp]
    public void SetUp()
    {
        _summonerRepository = new SummonerRepository(ApplicationDbContext);
    }
    
    [Test]
    public async Task GetByIdAsync_SummonerWithIdExists_ReturnsSummoner()
    {
        Summoner summoner = CreateSummoner();

        await ApplicationDbContext.AddAsync(summoner);
        await ApplicationDbContext.SaveChangesAsync();

        Summoner? summonerFromRepo = await _summonerRepository.GetByIdAsync(summoner.Id);
        
        Assert.That(summonerFromRepo, Is.Not.Null);
        Assert.That(summonerFromRepo, Is.EqualTo(summoner));
        Assert.That(ApplicationDbContext.Entry(summonerFromRepo).Collection(s => s.SummonerChampionMasteries).IsLoaded, Is.False);
    }
    
    [Test]
    public async Task GetByIdAsync_SummonerWithIdDoesNotExist_ReturnsNull()
    {
        Summoner? summonerFromRepo = await _summonerRepository.GetByIdAsync(Guid.NewGuid());
        
        Assert.That(summonerFromRepo, Is.Null);
    }
    
    [Test]
    public async Task AddAsync_AllValid_AddsSummoner()
    {
        Summoner summoner = CreateSummoner();

        await _summonerRepository.AddAsync(summoner);
        
        IEnumerable<Summoner> summonersInDb = await ApplicationDbContext.Summoners.ToListAsync();
        
        Assert.That(summonersInDb.Count(), Is.EqualTo(1));
        Assert.That(summonersInDb.Single(), Is.EqualTo(summoner));
    }
    
    [Test]
    public async Task AddRangeAsync_AllValid_AddsSummoners()
    {
        Summoner summoner1 = CreateSummoner();
        Summoner summoner2 = CreateSummoner();

        await _summonerRepository.AddRangeAsync(new []{ summoner1, summoner2});
        
        IEnumerable<Summoner> summonersInDb = await ApplicationDbContext.Summoners.ToListAsync();
        
        Assert.That(summonersInDb.Count(), Is.EqualTo(2));
        Assert.That(summonersInDb.Contains(summoner1), Is.True);
        Assert.That(summonersInDb.Contains(summoner1), Is.True);
    }
    
    [Test]
    public async Task DeleteAsync_SummonerExists_DeletesSummoner()
    {
        Summoner summoner = CreateSummoner();

        await ApplicationDbContext.AddAsync(summoner);
        await ApplicationDbContext.SaveChangesAsync();

        await _summonerRepository.DeleteAsync(summoner);

        Summoner? summonerFromDb = await ApplicationDbContext.Summoners.SingleOrDefaultAsync(s => s.Id == summoner.Id);
        
        Assert.That(summonerFromDb, Is.Null);
    }
    
    [Test]
    public async Task UpdateAsync_AllValid_UpdatesSummoner()
    {
        Summoner summoner = CreateSummoner();

        await ApplicationDbContext.AddAsync(summoner);
        await ApplicationDbContext.SaveChangesAsync();

        const int newProfileIcon = 10;
        const long newSummonerLevel = 999;
        Instant newLastUpdated = Instant.FromUtc(2024, 3, 27, 12, 0, 0);
        summoner.Update(
            newProfileIcon,
            newSummonerLevel,
            Enumerable.Empty<UpdateChampionMasteryDto>(),
            newLastUpdated);
        
        await _summonerRepository.UpdateAsync(summoner);

        Summoner? summonerFromDb = await ApplicationDbContext.Summoners.SingleOrDefaultAsync(s => s.Id == summoner.Id);
        
        Assert.That(summonerFromDb, Is.Not.Null);
        Assert.That(summonerFromDb, Is.EqualTo(summoner));
    }
    
    [Test]
    public async Task GetByIdWithAllIncludesAsync_SummonerWithIdExists_ReturnsSummoner()
    {
        Summoner summoner = CreateSummoner();

        await ApplicationDbContext.AddAsync(summoner);
        await ApplicationDbContext.SaveChangesAsync();

        Summoner? summonerFromRepo = await _summonerRepository.GetByIdWithAllIncludesAsync(summoner.Id);
        
        Assert.That(summonerFromRepo, Is.Not.Null);
        Assert.That(summonerFromRepo, Is.EqualTo(summoner));
        Assert.That(ApplicationDbContext.Entry(summonerFromRepo).Collection(s => s.SummonerChampionMasteries).IsLoaded, Is.True);
    }
    
    [Test]
    public async Task GetByIdWithAllIncludesAsync_SummonerWithIdDoesNotExist_ReturnsNull()
    {
        Summoner? summonerFromRepo = await _summonerRepository.GetByIdWithAllIncludesAsync(Guid.NewGuid());
        
        Assert.That(summonerFromRepo, Is.Null);
    }
    
        
    [Test]
    public async Task GetByPuuidAsync_SummonerWithIdExists_ReturnsSummoner()
    {
        Summoner summoner = CreateSummoner();

        await ApplicationDbContext.AddAsync(summoner);
        await ApplicationDbContext.SaveChangesAsync();

        Summoner? summonerFromRepo = await _summonerRepository.GetByPuuidAsync(summoner.Puuid);
        
        Assert.That(summonerFromRepo, Is.Not.Null);
        Assert.That(summonerFromRepo, Is.EqualTo(summoner));
        Assert.That(ApplicationDbContext.Entry(summonerFromRepo).Collection(s => s.SummonerChampionMasteries).IsLoaded, Is.False);
    }
    
    [Test]
    public async Task GetByPuuidAsync_SummonerWithPuuidDoesNotExist_ReturnsNull()
    {
        Summoner? summonerFromRepo = await _summonerRepository.GetByPuuidAsync("random puuid");
        
        Assert.That(summonerFromRepo, Is.Null);
    }
    
    [Test]
    public async Task GetByPuuidsAsync_SummonersWithPuuidsExists_ReturnsAllSummoner()
    {
        Summoner summoner1 = CreateSummoner();
        Summoner summoner2 = CreateSummoner();

        await ApplicationDbContext.AddRangeAsync(summoner1, summoner2);
        await ApplicationDbContext.SaveChangesAsync();

        List<string> puuids = new()
        {
            summoner1.Puuid,
            summoner2.Puuid
        };
        List<Summoner> summonerFromRepo = (await _summonerRepository.GetByPuuidsAsync(puuids)).ToList();
        
        Assert.That(summonerFromRepo.Count(), Is.EqualTo(2));
        Assert.That(summonerFromRepo.Contains(summoner1), Is.True);
        Assert.That(summonerFromRepo.Contains(summoner2), Is.True);
        Assert.That(ApplicationDbContext.Entry(summonerFromRepo.ElementAt(0)).Collection(s => s.SummonerChampionMasteries).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(summonerFromRepo.ElementAt(1)).Collection(s => s.SummonerChampionMasteries).IsLoaded, Is.False);
    }
    
    [Test]
    public async Task GetByPuuidsAsync_SummonerWithSomePuuidsExists_ReturnsSomeSummoner()
    {
        Summoner summoner1 = CreateSummoner();
        Summoner summoner2 = CreateSummoner();

        await ApplicationDbContext.AddRangeAsync(summoner1, summoner2);
        await ApplicationDbContext.SaveChangesAsync();

        List<string> puuids = new()
        {
            summoner1.Puuid,
            "notvalid"
        };
        List<Summoner> summonerFromRepo = (await _summonerRepository.GetByPuuidsAsync(puuids)).ToList();
        
        Assert.That(summonerFromRepo.Count(), Is.EqualTo(1));
        Assert.That(summonerFromRepo.Contains(summoner1), Is.True);
        Assert.That(summonerFromRepo.Contains(summoner2), Is.False);
        Assert.That(ApplicationDbContext.Entry(summonerFromRepo.ElementAt(0)).Collection(s => s.SummonerChampionMasteries).IsLoaded, Is.False);
    }
    
    [Test]
    public async Task GetByPuuidsAsync_SummonerWithNonePuuidsExists_ReturnsEmptyListOfSummoners()
    {
        Summoner summoner1 = CreateSummoner();
        Summoner summoner2 = CreateSummoner();

        await ApplicationDbContext.AddRangeAsync(summoner1, summoner2);
        await ApplicationDbContext.SaveChangesAsync();

        List<string> puuids = new()
        {
            "notvalid1",
            "notvalid2"
        };
        List<Summoner> summonerFromRepo = (await _summonerRepository.GetByPuuidsAsync(puuids)).ToList();
        
        Assert.That(summonerFromRepo, Is.Empty);
        Assert.That(summonerFromRepo.Contains(summoner1), Is.False);
        Assert.That(summonerFromRepo.Contains(summoner2), Is.False);
    }
    
    private Summoner CreateSummoner()
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
        Instant lastUpdated = Instant.MaxValue;

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
        
        Summoner summoner = new Summoner(
            summonerId,
            accountId,
            name,
            profileIconId,
            puuid,
            summonerLevel,
            gameName,
            tagLine,
            region,
            updateChampionMasteryDtos,
            lastUpdated);

        return summoner;
    }
}