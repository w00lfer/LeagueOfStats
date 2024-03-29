using AutoBogus;
using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using LeagueOfStats.Infrastructure.Matches;
using LeagueOfStats.Integration.Tests.TestCommons;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LeagueOfStats.Integration.Tests.Matches;

[TestFixture]
public class MatchRepositoryTests : IntegrationTestBase
{
    private MatchRepository _matchRepository;

    [SetUp]
    public void SetUp()
    {
        _matchRepository = new MatchRepository(ApplicationDbContext);
    }

    [Test]
    public async Task GetByIdAsync_MatchWithIdExists_ReturnsMatch()
    {
        Match match = CreateMatch();

        await ApplicationDbContext.AddAsync(match);
        await ApplicationDbContext.SaveChangesAsync();

        Match? matchFromRepo = await _matchRepository.GetByIdAsync(match.Id);

        Assert.That(matchFromRepo, Is.Not.Null);
        Assert.That(matchFromRepo, Is.EqualTo(match));
        Assert.That(ApplicationDbContext.Entry(matchFromRepo).Collection(m => m.Participants).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo).Collection(m => m.Teams).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetByIdAsync_MatchWithIdDoesNotExist_ReturnsNull()
    {
        Match? matchFromRepo = await _matchRepository.GetByIdAsync(Guid.NewGuid());

        Assert.That(matchFromRepo, Is.Null);
    }

    [Test]
    public async Task GetAllAsync_IdsAreEmpty_ReturnsAllMatches()
    {
        Match match1 = CreateMatch();
        Match match2 = CreateMatch();

        await ApplicationDbContext.AddRangeAsync(match1, match2);
        await ApplicationDbContext.SaveChangesAsync();

        List<Match> matchFromRepo = (await _matchRepository.GetAllAsync()).ToList();

        Assert.That(matchFromRepo.Count(), Is.EqualTo(2));
        Assert.That(matchFromRepo.Contains(match1), Is.True);
        Assert.That(matchFromRepo.Contains(match2), Is.True);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(0)).Collection(m => m.Participants).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(0)).Collection(m => m.Teams).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(1)).Collection(m => m.Participants).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(1)).Collection(m => m.Teams).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndAllMatchesWithGivenIdsExist_ReturnsAllMatchesWithGivenIds()
    {
        Match match1 = CreateMatch();
        Match match2 = CreateMatch();
        Match match3 = CreateMatch();

        await ApplicationDbContext.AddRangeAsync(match1, match2, match3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] matchIds =
        {
            match1.Id,
            match3.Id
        };
        List<Match> matchFromRepo = (await _matchRepository.GetAllAsync(matchIds)).ToList();

        Assert.That(matchFromRepo.Count(), Is.EqualTo(2));
        Assert.That(matchFromRepo.Contains(match1), Is.True);
        Assert.That(matchFromRepo.Contains(match3), Is.True);
        Assert.That(matchFromRepo.Contains(match2), Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(0)).Collection(m => m.Participants).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(0)).Collection(m => m.Teams).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(1)).Collection(m => m.Participants).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(1)).Collection(m => m.Teams).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndSomeMatchesWithGivenIdsExist_ReturnsSomeMatchesWithGivenIds()
    {
        Match match1 = CreateMatch();
        Match match2 = CreateMatch();
        Match match3 = CreateMatch();

        await ApplicationDbContext.AddRangeAsync(match1, match2, match3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] matchIds =
        {
            match1.Id,
            match2.Id,
            Faker.Random.Guid()
        };
        List<Match> matchFromRepo = (await _matchRepository.GetAllAsync(matchIds)).ToList();

        Assert.That(matchFromRepo.Count(), Is.EqualTo(2));
        Assert.That(matchFromRepo.Contains(match1), Is.True);
        Assert.That(matchFromRepo.Contains(match2), Is.True);
        Assert.That(matchFromRepo.Contains(match3), Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(0)).Collection(m => m.Participants).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(0)).Collection(m => m.Teams).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(1)).Collection(m => m.Participants).IsLoaded, Is.False);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo.ElementAt(1)).Collection(m => m.Teams).IsLoaded, Is.False);
    }

    [Test]
    public async Task GetAllAsync_IdsNotEmptyAndNoneMatchesWithGivenIdsExist_ReturnsEmptyListOfMatches()
    {
        Match match1 = CreateMatch();
        Match match2 = CreateMatch();
        Match match3 = CreateMatch();

        await ApplicationDbContext.AddRangeAsync(match1, match2, match3);
        await ApplicationDbContext.SaveChangesAsync();

        Guid[] matchIds =
        {
            Faker.Random.Guid(),
            Faker.Random.Guid()
        };
        List<Match> matchFromRepo = (await _matchRepository.GetAllAsync(matchIds)).ToList();

        Assert.That(matchFromRepo, Is.Empty);
    }

    [Test]
    public async Task AddAsync_AllValid_AddsMatch()
    {
        Match match = CreateMatch();

        await _matchRepository.AddAsync(match);

        IEnumerable<Match> matchesInDb = await ApplicationDbContext.Matches.ToListAsync();

        Assert.That(matchesInDb.Count(), Is.EqualTo(1));
        Assert.That(matchesInDb.Single(), Is.EqualTo(match));
    }

    [Test]
    public async Task AddRangeAsync_AllValid_AddsMatches()
    {
        Match match1 = CreateMatch();
        Match match2 = CreateMatch();

        await _matchRepository.AddRangeAsync(new[] { match1, match2 });

        IEnumerable<Match> matchesInDb = await ApplicationDbContext.Matches.ToListAsync();

        Assert.That(matchesInDb.Count(), Is.EqualTo(2));
        Assert.That(matchesInDb.Contains(match1), Is.True);
        Assert.That(matchesInDb.Contains(match1), Is.True);
    }

    [Test]
    public async Task DeleteAsync_MatchExists_DeletesMatch()
    {
        Match match = CreateMatch();

        await ApplicationDbContext.AddAsync(match);
        await ApplicationDbContext.SaveChangesAsync();

        await _matchRepository.DeleteAsync(match);

        Match? matchFromDb = await ApplicationDbContext.Matches.SingleOrDefaultAsync(m => m.Id == match.Id);

        Assert.That(matchFromDb, Is.Null);
    }
    
    [Test]
    public async Task GetByIdWithAllIncludesAsync_MatchWithIdExists_ReturnsMatch()
    {
        Match match = CreateMatch();

        await ApplicationDbContext.AddAsync(match);
        await ApplicationDbContext.SaveChangesAsync();

        Match? matchFromRepo = await _matchRepository.GetByIdWithAllIncludesAsync(match.Id);
        
        Assert.That(matchFromRepo, Is.Not.Null);
        Assert.That(matchFromRepo, Is.EqualTo(match));
        Assert.That(ApplicationDbContext.Entry(matchFromRepo).Collection(m => m.Teams).IsLoaded, Is.True);
        Assert.That(ApplicationDbContext.Entry(matchFromRepo).Collection(m => m.Participants).IsLoaded, Is.True);
    }
    
    [Test]
    public async Task GetByIdWithAllIncludesAsync_MatchWithIdDoesNotExist_ReturnsNull()
    {
        Match? matchFromRepo = await _matchRepository.GetByIdWithAllIncludesAsync(Guid.NewGuid());
        
        Assert.That(matchFromRepo, Is.Null);
    }
    
    [Test]
    public async Task GetAllByRiotMatchIdsWithAllIncludesAsync_MatchesWithRiotMatchIdsExists_ReturnsAllMatches()
    {
        Match match1 = CreateMatch();
        Match match2 = CreateMatch();

        await ApplicationDbContext.AddRangeAsync(match1, match2);
        await ApplicationDbContext.SaveChangesAsync();

        List<string> riotMatchIds = new()
        {
            match1.RiotMatchId,
            match2.RiotMatchId
        };
        List<Match> matchesFromRepo = (await _matchRepository.GetAllByRiotMatchIdsWithAllIncludesAsync(riotMatchIds)).ToList();

        Assert.That(matchesFromRepo.Count(), Is.EqualTo(2));
        Assert.That(matchesFromRepo.Contains(match1), Is.True);
        Assert.That(matchesFromRepo.Contains(match2), Is.True);
        Assert.That(ApplicationDbContext.Entry(matchesFromRepo.ElementAt(0)).Collection(m => m.Participants).IsLoaded, Is.True);
        Assert.That(ApplicationDbContext.Entry(matchesFromRepo.ElementAt(0)).Collection(m => m.Teams).IsLoaded, Is.True);
        Assert.That(ApplicationDbContext.Entry(matchesFromRepo.ElementAt(1)).Collection(m => m.Participants).IsLoaded, Is.True);
        Assert.That(ApplicationDbContext.Entry(matchesFromRepo.ElementAt(1)).Collection(m => m.Teams).IsLoaded, Is.True);
    }

    [Test]
    public async Task GetAllByRiotMatchIdsWithAllIncludesAsync_MatchWithSomeRiotMatchIdsExists_ReturnsSomeMatches()
    {
        Match match1 = CreateMatch();
        Match match2 = CreateMatch();

        await ApplicationDbContext.AddRangeAsync(match1, match2);
        await ApplicationDbContext.SaveChangesAsync();

        List<string> riotMatchIds = new()
        {
            match1.RiotMatchId,
            "invalidRiotMatchId"
        };
        List<Match> matchesFromRepo = (await _matchRepository.GetAllByRiotMatchIdsWithAllIncludesAsync(riotMatchIds)).ToList();

        Assert.That(matchesFromRepo.Count(), Is.EqualTo(1));
        Assert.That(matchesFromRepo.Contains(match1), Is.True);
        Assert.That(matchesFromRepo.Contains(match2), Is.False);
        Assert.That(ApplicationDbContext.Entry(matchesFromRepo.ElementAt(0)).Collection(m => m.Participants).IsLoaded, Is.True);
        Assert.That(ApplicationDbContext.Entry(matchesFromRepo.ElementAt(0)).Collection(m => m.Teams).IsLoaded, Is.True);
    }

    [Test]
    public async Task GetAllByRiotMatchIdsWithAllIncludesAsync_MatchWithNoneRiotMatchIdsExists_ReturnsEmptyListOfMatches()
    {
        Match match1 = CreateMatch();
        Match match2 = CreateMatch();

        await ApplicationDbContext.AddRangeAsync(match1, match2);
        await ApplicationDbContext.SaveChangesAsync();

        List<string> riotMatchIds = new()
        {
            "invalidRiotMatchId1",
            "invalidRiotMatchId2"
        };
        List<Match> matchesFromRepo = (await _matchRepository.GetAllByRiotMatchIdsWithAllIncludesAsync(riotMatchIds)).ToList();

        Assert.That(matchesFromRepo, Is.Empty);
        Assert.That(matchesFromRepo.Contains(match1), Is.False);
        Assert.That(matchesFromRepo.Contains(match2), Is.False);
    }

    private Match CreateMatch()
    {
        AddMatchDto addMatchDto = AutoFaker.Generate<AddMatchDto>();
        addMatchDto = addMatchDto with
        {
            AddParticipantDtos = Enumerable.Empty<AddParticipantDto>(),
            AddTeamDtos = Enumerable.Empty<AddTeamDto>()
        };

        Match match = new Match(addMatchDto);

        return match;
    }
}