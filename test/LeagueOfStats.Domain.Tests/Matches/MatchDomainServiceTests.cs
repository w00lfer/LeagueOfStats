using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Matches;
using Moq;
using NUnit.Framework;
using Match = LeagueOfStats.Domain.Matches.Match;

namespace LeagueOfStats.Domain.Tests.Matches;

[TestFixture]
public class MatchDomainServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepository = new();

    private MatchDomainService _matchDomainService;

    [SetUp]
    public void SetUp()
    {
        _matchRepository.Reset();
        
        _matchDomainService = new MatchDomainService(_matchRepository.Object);
    }
    
    [Test]
    public async Task GetByIdAsync_MatchDoesNotExist_ReturnsEntityNotFoundError()
    {
        Guid invalidMatchId = Guid.NewGuid();
        _matchRepository
            .Setup(x => x.GetByIdWithAllIncludesAsync(invalidMatchId))
            .ReturnsAsync((Match)null);

        Result<Match> result = await _matchDomainService.GetByIdAsync(invalidMatchId);
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<EntityNotFoundError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo($"Match with Id={invalidMatchId} does not exist."));
        
        _matchRepository.Verify(x => x.GetByIdWithAllIncludesAsync(invalidMatchId), Times.Once);
        _matchRepository.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetByIdAsync_MatchExists_ReturnsMatch()
    {
        Guid matchId = Guid.NewGuid();
        Match match = Mock.Of<Match>(d => d.Id == matchId);
        _matchRepository
            .Setup(x => x.GetByIdWithAllIncludesAsync(matchId))
            .ReturnsAsync(match);

        Result<Match> result = await _matchDomainService.GetByIdAsync(matchId);
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(match));
        
        _matchRepository.Verify(x => x.GetByIdWithAllIncludesAsync(matchId), Times.Once); 
        _matchRepository.VerifyNoOtherCalls();
    }

    // TODO Write this test after check is implemented
    [Test]
    public async Task AddMatchesAsync_NumberOfSummonersInMatchIsInvalid_ReturnsDomainError()
    {
    }
    
    [Test]
    public async Task AddMatchesAsync_AllValid_CreatesMatchesAndCallsRepoAddRangeAsyncAndReturnsMatches()
    {
        
    }
}