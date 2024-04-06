using Camille.RiotGames.AccountV1;
using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.SpectatorV4;
using Camille.RiotGames.Util;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.API.Infrastructure.ApiClients.RiotClient;
using LeagueOfStats.Application.ApiClients.RiotClient;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Application.Summoners.Enums;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using Moq;
using NodaTime;
using NUnit.Framework;
using Match = Camille.RiotGames.MatchV5.Match;
using Summoner = Camille.RiotGames.SummonerV4.Summoner;

namespace LeagueOfStats.API.Tests.Infrastructure.ApiClients.RiotClients;

[TestFixture]
public class RiotClientTests
{
    private readonly Mock<IRiotGamesApiWrapper> _riotGamesApiWrapperMock = new();

    private RiotClient _riotClient;

    [SetUp]
    public void SetUp()
    {
        _riotGamesApiWrapperMock.Reset();

        _riotClient = new(_riotGamesApiWrapperMock.Object);
    }

    [Test]
    public async Task GetSummonerByPuuidAsync_ApiWrapperThrowsException_ReturnsApiError()
    {
        const string puuid = "puuid";
        const Region region = Region.EUNE;

        _riotGamesApiWrapperMock
            .Setup(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null))
            .ThrowsAsync(new RiotResponseException(null));

        Result<Summoner> result = await _riotClient.GetSummonerByPuuidAsync(puuid, region);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("There are problems on Riot API side."));

        _riotGamesApiWrapperMock.Verify(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerByPuuidAsync_ApiWrapperReturnsNull_ReturnsApiError()
    {
        const string puuid = "puuid";
        const Region region = Region.EUNE;

        _riotGamesApiWrapperMock
            .Setup(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null))
            .ReturnsAsync((Summoner?)null);

        Result<Summoner> result = await _riotClient.GetSummonerByPuuidAsync(puuid, region);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(
            result.Errors.Single().ErrorMessage,
            Is.EqualTo($"Summoner with Puuid={puuid} and Region={region.ToString()} does not exist."));

        _riotGamesApiWrapperMock.Verify(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerByPuuidAsync_AllValid_ReturnsSummoner()
    {
        const string puuid = "puuid";
        const Region region = Region.EUNE;

        Summoner summoner = Mock.Of<Summoner>();
        _riotGamesApiWrapperMock
            .Setup(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null))
            .ReturnsAsync(summoner);

        Result<Summoner> result = await _riotClient.GetSummonerByPuuidAsync(puuid, region);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(summoner));

        _riotGamesApiWrapperMock.Verify(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task
        GetSummonerByGameNameAndTaglineAsync_ApiWrapperGetAccountByRiotIdAsyncThrowsException_ReturnsApiError()
    {
        const string gameName = "gameName";
        const string tagLine = "tagLine";
        const Region region = Region.EUNE;

        _riotGamesApiWrapperMock
            .Setup(x => x.GetAccountByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine, null))
            .ThrowsAsync(new RiotResponseException(null));

        Result<Summoner> result = await _riotClient.GetSummonerByGameNameAndTaglineAsync(gameName, tagLine, region);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("There are problems on Riot API side."));

        _riotGamesApiWrapperMock.Verify(x =>
            x.GetAccountByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task
        GetSummonerByGameNameAndTaglineAsync_ApiWrapperGetAccountByRiotIdAsyncReturnsNull_ReturnsApiError()
    {
        const string gameName = "gameName";
        const string tagLine = "tagLine";
        const Region region = Region.EUNE;

        _riotGamesApiWrapperMock
            .Setup(x => x.GetAccountByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine, null))
            .ReturnsAsync((Account?)null);

        Result<Summoner> result = await _riotClient.GetSummonerByGameNameAndTaglineAsync(gameName, tagLine, region);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage,
            Is.EqualTo($"There is no such account: {gameName}#{tagLine}."));

        _riotGamesApiWrapperMock.Verify(x =>
            x.GetAccountByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerByGameNameAndTaglineAsync_ApiWrapperGetSummonerByPuuidThrowsException_ReturnsApiError()
    {
        const string gameName = "gameName";
        const string tagLine = "tagLine";
        const string puuid = "puuid";
        const Region region = Region.EUNE;

        Account account = Mock.Of<Account>(a => a.Puuid == puuid);
        _riotGamesApiWrapperMock
            .Setup(x => x.GetAccountByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine, null))
            .ReturnsAsync(account);

        _riotGamesApiWrapperMock
            .Setup(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null))
            .ThrowsAsync(new RiotResponseException(null));

        Result<Summoner> result = await _riotClient.GetSummonerByGameNameAndTaglineAsync(gameName, tagLine, region);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("There are problems on Riot API side."));

        _riotGamesApiWrapperMock.Verify(x =>
            x.GetAccountByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine, null));
        _riotGamesApiWrapperMock.Verify(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerByGameNameAndTaglineAsync_ApiWrapperGetSummonerByPuuidReturnsNull_ReturnsApiError()
    {
        const string gameName = "gameName";
        const string tagLine = "tagLine";
        const string puuid = "puuid";
        const Region region = Region.EUNE;

        Account account = Mock.Of<Account>(a => a.Puuid == puuid);
        _riotGamesApiWrapperMock
            .Setup(x => x.GetAccountByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine, null))
            .ReturnsAsync(account);

        _riotGamesApiWrapperMock
            .Setup(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null))
            .ReturnsAsync((Summoner?)null);

        Result<Summoner> result = await _riotClient.GetSummonerByGameNameAndTaglineAsync(gameName, tagLine, region);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(
            result.Errors.Single().ErrorMessage,
            Is.EqualTo($"Summoner with RiotId={gameName}#{tagLine} and Region={region.ToString()} does not exist."));

        _riotGamesApiWrapperMock.Verify(x =>
            x.GetAccountByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine, null));
        _riotGamesApiWrapperMock.Verify(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerByGameNameAndTaglineAsync_AllValid_ReturnsSummoner()
    {
        const string gameName = "gameName";
        const string tagLine = "tagLine";
        const string puuid = "puuid";
        const Region region = Region.EUNE;

        Account account = Mock.Of<Account>(a => a.Puuid == puuid);
        _riotGamesApiWrapperMock
            .Setup(x => x.GetAccountByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine, null))
            .ReturnsAsync(account);

        Summoner summoner = Mock.Of<Summoner>();
        _riotGamesApiWrapperMock
            .Setup(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null))
            .ReturnsAsync(summoner);

        Result<Summoner> result = await _riotClient.GetSummonerByGameNameAndTaglineAsync(gameName, tagLine, region);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(summoner));

        _riotGamesApiWrapperMock.Verify(x =>
            x.GetAccountByRiotIdAsync(region.ToRegionalRoute(), gameName, tagLine, null));
        _riotGamesApiWrapperMock.Verify(x => x.GetSummonerByPuuidAsync(region.ToPlatformRoute(), puuid, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerChampionMasteryByPuuidAsync_ApiWrapperThrowsException_ReturnsApiError()
    {
        const string puuid = "puuid";
        const Region region = Region.EUNE;

        _riotGamesApiWrapperMock
            .Setup(x => x.GetChampionMasteryByPuuid(region.ToPlatformRoute(), puuid, null))
            .ThrowsAsync(new RiotResponseException(null));

        Result<ChampionMastery[]> result = await _riotClient.GetSummonerChampionMasteryByPuuidAsync(puuid, region);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("There are problems on Riot API side."));

        _riotGamesApiWrapperMock.Verify(x => x.GetChampionMasteryByPuuid(region.ToPlatformRoute(), puuid, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerChampionMasteryByPuuidAsync_ApiWrapperReturnsNull_ReturnsApiError()
    {
        const string puuid = "puuid";
        const Region region = Region.EUNE;

        _riotGamesApiWrapperMock
            .Setup(x => x.GetChampionMasteryByPuuid(region.ToPlatformRoute(), puuid, null))
            .ReturnsAsync((ChampionMastery[])null);

        Result<ChampionMastery[]> result = await _riotClient.GetSummonerChampionMasteryByPuuidAsync(puuid, region);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(
            result.Errors.Single().ErrorMessage,
            Is.EqualTo(
                $"Summoner with Puuid={puuid} and Region={region} neither does not exist or has no champion masteries."));

        _riotGamesApiWrapperMock.Verify(x => x.GetChampionMasteryByPuuid(region.ToPlatformRoute(), puuid, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerChampionMasteryByPuuidAsync_AllValid_ReturnsChampionMasteries()
    {
        const string puuid = "puuid";
        const Region region = Region.EUNE;

        ChampionMastery[] championMasteries = Array.Empty<ChampionMastery>();
        _riotGamesApiWrapperMock
            .Setup(x => x.GetChampionMasteryByPuuid(region.ToPlatformRoute(), puuid, null))
            .ReturnsAsync(championMasteries);

        Result<ChampionMastery[]> result = await _riotClient.GetSummonerChampionMasteryByPuuidAsync(puuid, region);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(championMasteries));

        _riotGamesApiWrapperMock.Verify(x => x.GetChampionMasteryByPuuid(region.ToPlatformRoute(), puuid, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerMatchHistorySummaryAsync_ApiWrapperGetMatchIdsByPuuidThrowsException_ReturnsApiError()
    {
        const Region region = Region.EUNE;
        const string puuid = "puuid";
        const int count = 10;
        const MatchHistoryQueueFilter matchHistoryQueueFilter = MatchHistoryQueueFilter.All;
        Instant gameEndedAt = Instant.MaxValue;
        GetSummonerMatchHistoryDto getSummonerMatchHistoryDto =
            new(region, puuid, count, gameEndedAt, matchHistoryQueueFilter);

        _riotGamesApiWrapperMock
            .Setup(x => x.GetMatchIdsByPuuidAsync(
                region.ToRegionalRoute(),
                puuid,
                count,
                gameEndedAt.ToUnixTimeSeconds(),
                matchHistoryQueueFilter.ToNullableQueue(),
                null,
                null,
                null,
                null))
            .ThrowsAsync(new RiotResponseException(null));

        Result<IEnumerable<Match>> result =
            await _riotClient.GetSummonerMatchHistorySummaryAsync(getSummonerMatchHistoryDto);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("There are problems on Riot API side."));

        _riotGamesApiWrapperMock.Verify(x => x.GetMatchIdsByPuuidAsync(
            region.ToRegionalRoute(),
            puuid,
            count,
            gameEndedAt.ToUnixTimeSeconds(),
            matchHistoryQueueFilter.ToNullableQueue(),
            null,
            null,
            null,
            null), Times.Once);
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerMatchHistorySummaryAsync_ApiWrapperGetMatchByIdThrowsException_ReturnsApiError()
    {
        const Region region = Region.EUNE;
        const string puuid = "puuid";
        const int count = 10;
        const MatchHistoryQueueFilter matchHistoryQueueFilter = MatchHistoryQueueFilter.All;
        Instant gameEndedAt = Instant.MaxValue;
        GetSummonerMatchHistoryDto getSummonerMatchHistoryDto =
            new(region, puuid, count, gameEndedAt, matchHistoryQueueFilter);

        const string matchId = "matchId";
        string[] matchIds = { matchId };

        _riotGamesApiWrapperMock
            .Setup(x => x.GetMatchIdsByPuuidAsync(
                region.ToRegionalRoute(),
                puuid,
                count,
                gameEndedAt.ToUnixTimeSeconds(),
                matchHistoryQueueFilter.ToNullableQueue(),
                null,
                null,
                null,
                null))
            .ReturnsAsync(matchIds);

        _riotGamesApiWrapperMock
            .Setup(x => x.GetMatchByIdAsync(region.ToRegionalRoute(), matchId, null))
            .ThrowsAsync(new RiotResponseException(null));

        Result<IEnumerable<Match>> result =
            await _riotClient.GetSummonerMatchHistorySummaryAsync(getSummonerMatchHistoryDto);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("There are problems on Riot API side."));

        _riotGamesApiWrapperMock.Verify(x => x.GetMatchIdsByPuuidAsync(
            region.ToRegionalRoute(),
            puuid,
            count,
            gameEndedAt.ToUnixTimeSeconds(),
            matchHistoryQueueFilter.ToNullableQueue(),
            null,
            null,
            null,
            null), Times.Once);
        _riotGamesApiWrapperMock.Verify(x => x.GetMatchByIdAsync(region.ToRegionalRoute(), matchId, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerMatchHistorySummaryAsync_AllValid_ReturnsMatches()
    {
        const Region region = Region.EUNE;
        const string puuid = "puuid";
        const int count = 10;
        const MatchHistoryQueueFilter matchHistoryQueueFilter = MatchHistoryQueueFilter.All;
        Instant gameEndedAt = Instant.MaxValue;
        GetSummonerMatchHistoryDto getSummonerMatchHistoryDto =
            new(region, puuid, count, gameEndedAt, matchHistoryQueueFilter);

        const string matchId = "matchId";
        string[] matchIds = { matchId };

        _riotGamesApiWrapperMock
            .Setup(x => x.GetMatchIdsByPuuidAsync(
                region.ToRegionalRoute(),
                puuid,
                count,
                gameEndedAt.ToUnixTimeSeconds(),
                matchHistoryQueueFilter.ToNullableQueue(),
                null,
                null,
                null,
                null))
            .ReturnsAsync(matchIds);

        Match match = Mock.Of<Match>();
        _riotGamesApiWrapperMock
            .Setup(x => x.GetMatchByIdAsync(region.ToRegionalRoute(), matchId, null))
            .ReturnsAsync(match);

        Result<IEnumerable<Match>> result =
            await _riotClient.GetSummonerMatchHistorySummaryAsync(getSummonerMatchHistoryDto);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Contains(match), Is.True);

        _riotGamesApiWrapperMock.Verify(x => x.GetMatchIdsByPuuidAsync(
            region.ToRegionalRoute(),
            puuid,
            count,
            gameEndedAt.ToUnixTimeSeconds(),
            matchHistoryQueueFilter.ToNullableQueue(),
            null,
            null,
            null,
            null), Times.Once);
        _riotGamesApiWrapperMock.Verify(x => x.GetMatchByIdAsync(region.ToRegionalRoute(), matchId, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task GetSummonerLiveGameAsync_ApiWrapperThrowsException_ReturnsApiError()
    {
        const string riotSummonerId = "riotSummonerId";
        const Region region = Region.EUNE;
        SummonerName summonerName = SummonerName.Create("gameName", "tagLine");
        GetSummonerLiveGameDto getSummonerLiveGameDto = new(summonerName, riotSummonerId, region);

        _riotGamesApiWrapperMock
            .Setup(x => x.GetCurrentGameInfoBySummonerIdAsync(region.ToPlatformRoute(), riotSummonerId, null))
            .ThrowsAsync(new RiotResponseException(null));

        Result<CurrentGameInfo> result = await _riotClient.GetSummonerLiveGameAsync(getSummonerLiveGameDto);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("There are problems on Riot API side."));

        _riotGamesApiWrapperMock.Verify(x => x.GetCurrentGameInfoBySummonerIdAsync(region.ToPlatformRoute(), riotSummonerId, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerLiveGameAsync_ApiWrapperReturnsNull_ReturnsApiError()
    {
        const string riotSummonerId = "riotSummonerId";
        const Region region = Region.EUNE;
        SummonerName summonerName = SummonerName.Create("gameName", "tagLine");
        GetSummonerLiveGameDto getSummonerLiveGameDto = new(summonerName, riotSummonerId, region);

        _riotGamesApiWrapperMock
            .Setup(x => x.GetCurrentGameInfoBySummonerIdAsync(region.ToPlatformRoute(), riotSummonerId, null))
            .ReturnsAsync((CurrentGameInfo?)null);

        Result<CurrentGameInfo> result = await _riotClient.GetSummonerLiveGameAsync(getSummonerLiveGameDto);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(
            result.Errors.Single().ErrorMessage,
            Is.EqualTo($"Summoner={getSummonerLiveGameDto.SummonerName} is not in game."));

        _riotGamesApiWrapperMock.Verify(x => x.GetCurrentGameInfoBySummonerIdAsync(region.ToPlatformRoute(), riotSummonerId, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSummonerLiveGameAsync_AllValid_ReturnsSummoner()
    {
        const string riotSummonerId = "riotSummonerId";
        const Region region = Region.EUNE;
        SummonerName summonerName = SummonerName.Create("gameName", "tagLine");
        GetSummonerLiveGameDto getSummonerLiveGameDto = new(summonerName, riotSummonerId, region);

        CurrentGameInfo currentGameInfo = Mock.Of<CurrentGameInfo>();
        _riotGamesApiWrapperMock
            .Setup(x => x.GetCurrentGameInfoBySummonerIdAsync(region.ToPlatformRoute(), riotSummonerId, null))
            .ReturnsAsync(currentGameInfo);

        Result<CurrentGameInfo> result = await _riotClient.GetSummonerLiveGameAsync(getSummonerLiveGameDto);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(currentGameInfo));

        _riotGamesApiWrapperMock.Verify(x => x.GetCurrentGameInfoBySummonerIdAsync(region.ToPlatformRoute(), riotSummonerId, null));
        _riotGamesApiWrapperMock.VerifyNoOtherCalls();
    }
}