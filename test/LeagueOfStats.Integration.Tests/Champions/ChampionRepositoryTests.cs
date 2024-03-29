using AutoBogus;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Infrastructure.Champions;
using LeagueOfStats.Integration.Tests.TestCommons;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LeagueOfStats.Integration.Tests.Champions;

[TestFixture]
public class ChampionRepositoryTests : IntegrationTestBase
{
    private ChampionRepository _championRepository;

    [SetUp]
    public void SetUp()
    {
        _championRepository = new ChampionRepository(ApplicationDbContext);
    }

    [Test]
    public async Task GetByIdAsync_ChampionWithIdExists_ReturnsChampion()
    {
        Champion champion = CreateChampion();

        await ApplicationDbContext.AddAsync(champion);
        await ApplicationDbContext.SaveChangesAsync();

        Champion? championFromRepo = await _championRepository.GetByIdAsync(champion.Id);

        Assert.That(championFromRepo, Is.Not.Null);
        Assert.That(championFromRepo, Is.EqualTo(champion));
    }

    [Test]
    public async Task GetByIdAsync_ChampionWithIdDoesNotExist_ReturnsNull()
    {
        Champion? championFromRepo = await _championRepository.GetByIdAsync(Guid.NewGuid());

        Assert.That(championFromRepo, Is.Null);
    }

    [Test]
    public async Task GetAllAsync_IdsAreEmpty_ReturnsAllChampions()
    {
        Champion champion1 = CreateChampion();
        Champion champion2 = CreateChampion();

        await ApplicationDbContext.AddRangeAsync(champion1, champion2);
        await ApplicationDbContext.SaveChangesAsync();

        List<Champion> championFromRepo = (await _championRepository.GetAllAsync()).ToList();

        Assert.That(championFromRepo.Count(), Is.EqualTo(2));
        Assert.That(championFromRepo.Contains(champion1), Is.True);
        Assert.That(championFromRepo.Contains(champion2), Is.True);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndAllChampionsWithGivenIdsExist_ReturnsAllChampionsWithGivenIds()
    {
        Champion champion1 = CreateChampion();
        Champion champion2 = CreateChampion();
        Champion champion3 = CreateChampion();

        await ApplicationDbContext.AddRangeAsync(champion1, champion2, champion3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] championIds =
        {
            champion1.Id,
            champion3.Id
        };
        List<Champion> championFromRepo = (await _championRepository.GetAllAsync(championIds)).ToList();

        Assert.That(championFromRepo.Count(), Is.EqualTo(2));
        Assert.That(championFromRepo.Contains(champion1), Is.True);
        Assert.That(championFromRepo.Contains(champion3), Is.True);
        Assert.That(championFromRepo.Contains(champion2), Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndSomeChampionsWithGivenIdsExist_ReturnsSomeChampionsWithGivenIds()
    {
        Champion champion1 = CreateChampion();
        Champion champion2 = CreateChampion();
        Champion champion3 = CreateChampion();

        await ApplicationDbContext.AddRangeAsync(champion1, champion2, champion3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] championIds =
        {
            champion1.Id,
            champion2.Id,
            Faker.Random.Guid()
        };
        List<Champion> championFromRepo = (await _championRepository.GetAllAsync(championIds)).ToList();

        Assert.That(championFromRepo.Count(), Is.EqualTo(2));
        Assert.That(championFromRepo.Contains(champion1), Is.True);
        Assert.That(championFromRepo.Contains(champion2), Is.True);
        Assert.That(championFromRepo.Contains(champion3), Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndNoneChampionsWithGivenIdsExist_ReturnsEmptyListOfChampions()
    {
        Champion champion1 = CreateChampion();
        Champion champion2 = CreateChampion();
        Champion champion3 = CreateChampion();

        await ApplicationDbContext.AddRangeAsync(champion1, champion2, champion3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] championIds =
        {
            Faker.Random.Guid(),
            Faker.Random.Guid()
        };
        List<Champion> championFromRepo = (await _championRepository.GetAllAsync(championIds)).ToList();

        Assert.That(championFromRepo, Is.Empty);
    }

    [Test]
    public async Task AddAsync_AllValid_AddsChampion()
    {
        Champion champion = CreateChampion();

        await _championRepository.AddAsync(champion);

        IEnumerable<Champion> championsInDb = await ApplicationDbContext.Champions.ToListAsync();

        Assert.That(championsInDb.Count(), Is.EqualTo(1));
        Assert.That(championsInDb.Single(), Is.EqualTo(champion));
    }

    [Test]
    public async Task AddRangeAsync_AllValid_AddsChampions()
    {
        Champion champion1 = CreateChampion();
        Champion champion2 = CreateChampion();

        await _championRepository.AddRangeAsync(new[] { champion1, champion2 });

        IEnumerable<Champion> championsInDb = await ApplicationDbContext.Champions.ToListAsync();

        Assert.That(championsInDb.Count(), Is.EqualTo(2));
        Assert.That(championsInDb.Contains(champion1), Is.True);
        Assert.That(championsInDb.Contains(champion1), Is.True);
    }

    [Test]
    public async Task DeleteAsync_ChampionExists_DeletesChampion()
    {
        Champion champion = CreateChampion();

        await ApplicationDbContext.AddAsync(champion);
        await ApplicationDbContext.SaveChangesAsync();

        await _championRepository.DeleteAsync(champion);

        Champion? championFromDb = await ApplicationDbContext.Champions.SingleOrDefaultAsync(s => s.Id == champion.Id);

        Assert.That(championFromDb, Is.Null);
    }

    private Champion CreateChampion()
    {
        int riotChampionId = Faker.Random.Int();
        string name  = Faker.Lorem.Word();
        string title  = Faker.Lorem.Word();
        string description  = Faker.Lorem.Word();
        string splashUrl  = Faker.Lorem.Word();
        string uncenteredSplashUrl  = Faker.Lorem.Word();
        string iconUrl  = Faker.Lorem.Word();
        string tileUrl  = Faker.Lorem.Word();

        Champion champion = new(
            riotChampionId,
            name,
            title,
            description,
            splashUrl,
            uncenteredSplashUrl,
            iconUrl,
            tileUrl);

        return champion;
    }
}