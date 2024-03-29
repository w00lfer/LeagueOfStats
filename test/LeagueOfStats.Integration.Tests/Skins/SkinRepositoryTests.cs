using LeagueOfStats.Domain.Skins;
using LeagueOfStats.Infrastructure.Skins;
using LeagueOfStats.Integration.Tests.TestCommons;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LeagueOfStats.Integration.Tests.Skins;

[TestFixture]
public class SkinRepositoryTests : IntegrationTestBase
{
    private SkinRepository _skinRepository;

    [SetUp]
    public void SetUp()
    {
        _skinRepository = new SkinRepository(ApplicationDbContext);
    }

    [Test]
    public async Task GetByIdAsync_SkinWithIdExists_ReturnsSkin()
    {
        Skin skin = CreateSkin();

        await ApplicationDbContext.AddAsync(skin);
        await ApplicationDbContext.SaveChangesAsync();

        Skin? skinFromRepo = await _skinRepository.GetByIdAsync(skin.Id);

        Assert.That(skinFromRepo, Is.Not.Null);
        Assert.That(skinFromRepo, Is.EqualTo(skin));
        Assert.That(ApplicationDbContext.Entry(skinFromRepo).Collection(s => s.Chromas).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetByIdAsync_SkinWithIdDoesNotExist_ReturnsNull()
    {
        Skin? skinFromRepo = await _skinRepository.GetByIdAsync(Guid.NewGuid());

        Assert.That(skinFromRepo, Is.Null);
    }

    [Test]
    public async Task GetAllAsync_IdsAreEmpty_ReturnsAllSkins()
    {
        Skin skin1 = CreateSkin();
        Skin skin2 = CreateSkin();

        await ApplicationDbContext.AddRangeAsync(skin1, skin2);
        await ApplicationDbContext.SaveChangesAsync();

        List<Skin> skinFromRepo = (await _skinRepository.GetAllAsync()).ToList();

        Assert.That(skinFromRepo.Count(), Is.EqualTo(2));
        Assert.That(skinFromRepo.Contains(skin1), Is.True);
        Assert.That(skinFromRepo.Contains(skin2), Is.True);
        Assert.That(ApplicationDbContext.Entry(skinFromRepo.ElementAt(0)).Collection(s => s.Chromas).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(skinFromRepo.ElementAt(1)).Collection(s => s.Chromas).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndAllSkinsWithGivenIdsExist_ReturnsAllSkinsWithGivenIds()
    {
        Skin skin1 = CreateSkin();
        Skin skin2 = CreateSkin();
        Skin skin3 = CreateSkin();

        await ApplicationDbContext.AddRangeAsync(skin1, skin2, skin3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] skinIds =
        {
            skin1.Id,
            skin3.Id
        };
        List<Skin> skinFromRepo = (await _skinRepository.GetAllAsync(skinIds)).ToList();

        Assert.That(skinFromRepo.Count(), Is.EqualTo(2));
        Assert.That(skinFromRepo.Contains(skin1), Is.True);
        Assert.That(skinFromRepo.Contains(skin3), Is.True);
        Assert.That(skinFromRepo.Contains(skin2), Is.False);
        Assert.That(ApplicationDbContext.Entry(skinFromRepo.ElementAt(0)).Collection(s => s.Chromas).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(skinFromRepo.ElementAt(1)).Collection(s => s.Chromas).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndSomeSkinsWithGivenIdsExist_ReturnsSomeSkinsWithGivenIds()
    {
        Skin skin1 = CreateSkin();
        Skin skin2 = CreateSkin();
        Skin skin3 = CreateSkin();

        await ApplicationDbContext.AddRangeAsync(skin1, skin2, skin3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] skinIds =
        {
            skin1.Id,
            skin2.Id,
            Faker.Random.Guid()
        };
        List<Skin> skinFromRepo = (await _skinRepository.GetAllAsync(skinIds)).ToList();

        Assert.That(skinFromRepo.Count(), Is.EqualTo(2));
        Assert.That(skinFromRepo.Contains(skin1), Is.True);
        Assert.That(skinFromRepo.Contains(skin2), Is.True);
        Assert.That(skinFromRepo.Contains(skin3), Is.False);
        Assert.That(ApplicationDbContext.Entry(skinFromRepo.ElementAt(0)).Collection(s => s.Chromas).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(skinFromRepo.ElementAt(1)).Collection(s => s.Chromas).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndNoneSkinsWithGivenIdsExist_ReturnsEmptyListOfSkins()
    {
        Skin skin1 = CreateSkin();
        Skin skin2 = CreateSkin();
        Skin skin3 = CreateSkin();

        await ApplicationDbContext.AddRangeAsync(skin1, skin2, skin3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] skinIds =
        {
            Faker.Random.Guid(),
            Faker.Random.Guid()
        };
        List<Skin> skinFromRepo = (await _skinRepository.GetAllAsync(skinIds)).ToList();

        Assert.That(skinFromRepo, Is.Empty);
    }

    [Test]
    public async Task AddAsync_AllValid_AddsSkin()
    {
        Skin skin = CreateSkin();

        await _skinRepository.AddAsync(skin);

        IEnumerable<Skin> skinsInDb = await ApplicationDbContext.Skins.ToListAsync();

        Assert.That(skinsInDb.Count(), Is.EqualTo(1));
        Assert.That(skinsInDb.Single(), Is.EqualTo(skin));
    }

    [Test]
    public async Task AddRangeAsync_AllValid_AddsSkins()
    {
        Skin skin1 = CreateSkin();
        Skin skin2 = CreateSkin();

        await _skinRepository.AddRangeAsync(new[] { skin1, skin2 });

        IEnumerable<Skin> skinsInDb = await ApplicationDbContext.Skins.ToListAsync();

        Assert.That(skinsInDb.Count(), Is.EqualTo(2));
        Assert.That(skinsInDb.Contains(skin1), Is.True);
        Assert.That(skinsInDb.Contains(skin1), Is.True);
    }

    [Test]
    public async Task DeleteAsync_SkinExists_DeletesSkin()
    {
        Skin skin = CreateSkin();

        await ApplicationDbContext.AddAsync(skin);
        await ApplicationDbContext.SaveChangesAsync();

        await _skinRepository.DeleteAsync(skin);

        Skin? skinFromDb = await ApplicationDbContext.Skins.SingleOrDefaultAsync(s => s.Id == skin.Id);

        Assert.That(skinFromDb, Is.Null);
    }
    
    [Test]
    public async Task GetByRiotSkinIdsAsync_SkinsWithRiotSkinIdsExists_ReturnsAllSkins()
    {
        Skin skin1 = CreateSkin();
        Skin skin2 = CreateSkin();

        await ApplicationDbContext.AddRangeAsync(skin1, skin2);
        await ApplicationDbContext.SaveChangesAsync();

        List<int> riotSkinIds = new()
        {
            skin1.RiotSkinId,
            skin2.RiotSkinId
        };
        List<Skin> skinsFromRepo = (await _skinRepository.GetByRiotSkinIdsAsync(riotSkinIds)).ToList();

        Assert.That(skinsFromRepo.Count(), Is.EqualTo(2));
        Assert.That(skinsFromRepo.Contains(skin1), Is.True);
        Assert.That(skinsFromRepo.Contains(skin2), Is.True);
        Assert.That(ApplicationDbContext.Entry(skinsFromRepo.ElementAt(0)).Collection(s => s.Chromas).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(skinsFromRepo.ElementAt(1)).Collection(s => s.Chromas).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetByRiotSkinIdsAsync_SkinWithSomeRiotSkinIdsExists_ReturnsSomeSkins()
    {
        Skin skin1 = CreateSkin();
        Skin skin2 = CreateSkin();

        await ApplicationDbContext.AddRangeAsync(skin1, skin2);
        await ApplicationDbContext.SaveChangesAsync();

        List<int> riotSkinIds = new()
        {
            skin1.RiotSkinId,
            25
        };
        List<Skin> skinsFromRepo = (await _skinRepository.GetByRiotSkinIdsAsync(riotSkinIds)).ToList();

        Assert.That(skinsFromRepo.Count(), Is.EqualTo(1));
        Assert.That(skinsFromRepo.Contains(skin1), Is.True);
        Assert.That(skinsFromRepo.Contains(skin2), Is.False);
        Assert.That(ApplicationDbContext.Entry(skinsFromRepo.ElementAt(0)).Collection(s => s.Chromas).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetByRiotSkinIdsAsync_SkinWithNoneRiotSkinIdsExists_ReturnsEmptyListOfSkins()
    {
        Skin skin1 = CreateSkin();
        Skin skin2 = CreateSkin();

        await ApplicationDbContext.AddRangeAsync(skin1, skin2);
        await ApplicationDbContext.SaveChangesAsync();

        List<int> riotSkinIds = new()
        {
            10,
            888
        };
        List<Skin> skinsFromRepo = (await _skinRepository.GetByRiotSkinIdsAsync(riotSkinIds)).ToList();

        Assert.That(skinsFromRepo, Is.Empty);
        Assert.That(skinsFromRepo.Contains(skin1), Is.False);
        Assert.That(skinsFromRepo.Contains(skin2), Is.False);
    }

    private Skin CreateSkin()
    {
        int riotSkinId = Faker.Random.Int();
        bool isBase = Faker.Random.Bool();
        string name = Faker.Lorem.Word();
        string description = Faker.Lorem.Word();
        string splashUrl = Faker.Lorem.Word();
        string uncenteredSplashUrl = Faker.Lorem.Word();
        string tileUrl = Faker.Lorem.Word();
        string rarity = Faker.Lorem.Word();
        bool isLegacy = Faker.Random.Bool();
        string chromaPath = Faker.Lorem.Word();
        AddSkinDto addSkinDto = new(
            riotSkinId,
            isBase,
            name,
            description,
            splashUrl,
            uncenteredSplashUrl,
            tileUrl,
            rarity,
            isLegacy,
            chromaPath,
            Enumerable.Empty<AddSkinChromaDto>());

        Skin skin = new(addSkinDto);

        return skin;
    }
}
