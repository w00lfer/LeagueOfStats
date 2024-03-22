using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Domain.Summoners.Dtos;
using LeagueOfStats.Infrastructure.Summoners;
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
    public async Task s()
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

        await _summonerRepository.AddAsync(summoner);

        IEnumerable<Summoner> summonersInDb = await ApplicationDbContext.Summoners.ToListAsync();
        
        Assert.That(summonersInDb.Count(), Is.EqualTo(1));
        Assert.That(summonersInDb.Single(), Is.EqualTo(summoner));
    }
}