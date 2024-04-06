using Bogus;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.API.Infrastructure.ApiClients.CommunityDragonClient;
using LeagueOfStats.Application.ApiClients.CommunityDragonClient;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Jobs.ApiClients.CommunityDragonClient;
using Moq;
using Moq.Contrib.HttpClient;
using NUnit.Framework;

namespace LeagueOfStats.API.Tests.Infrastructure.ApiClients.CommunityDragonClients;

[TestFixture]
public class CommunityDragonClientTests
{
    private const string HttpClientBaseAddress = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/";
    private const string EndpointUrl = "skins.json";
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new ();

    private CommunityDragonClient _communityDragonClient;

    [SetUp]
    public void SetUp()
    {
        HttpClient httpClient = _httpMessageHandlerMock.CreateClient();
        httpClient.BaseAddress = new Uri(HttpClientBaseAddress);
        
        _communityDragonClient = new(httpClient);
    }

    [Test]
    public async Task GetSkinsAsync_GetSkinsEndpointReturnsNull_ReturnsApiError()
    {
        _httpMessageHandlerMock
            .SetupRequest(HttpMethod.Get, string.Concat(HttpClientBaseAddress, EndpointUrl))
            .ReturnsJsonResponse<Dictionary<string, CommunityDragonSkinDataDto>>(null);

        Result<IEnumerable<SkinDto>> result = await _communityDragonClient.GetSkinsAsync();
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("Community Dragon skins can't be accessed."));
    } 
    
    [Test]
    public async Task GetSkinsAsync_GetSkinsEndpointReturnsSkins_ReturnsResultSuccessWithSkinDtos()
    {
        Faker faker = new();
        int id = faker.Random.Int();
        bool isBase = faker.Random.Bool();
        string name = faker.Lorem.Word();
        string rarity = faker.Lorem.Word();
        bool isLegacy = faker.Random.Bool();
        string chromaPath = faker.Lorem.Word();
        string description = faker.Lorem.Word();

        int chromaId = faker.Random.Int();
        string chromaChromaPath = faker.Lorem.Word();
        List<string> colors = new()
        {
            faker.Lorem.Word(),
        };

        Dictionary<string, CommunityDragonSkinDataDto> communityDragonResponseData =
            new Dictionary<string, CommunityDragonSkinDataDto>()
            {
                {
                    "1", new CommunityDragonSkinDataDto(
                        id,
                        isBase,
                        name,
                        default,
                        default,
                        default,
                        default,
                        default,
                        default,
                        rarity,
                        isLegacy,
                        default,
                        default,
                        default,
                        chromaPath,
                        new List<CommunityDragonSkinDataChromaDto>()
                        {
                            new(
                                chromaId,
                                chromaChromaPath,
                                colors)
                        },
                        default,
                        default,
                        default,
                        default,
                        description)
                }
            };

        _httpMessageHandlerMock
            .SetupRequest(HttpMethod.Get, string.Concat(HttpClientBaseAddress, EndpointUrl))
            .ReturnsJsonResponse(communityDragonResponseData);

        Result<IEnumerable<SkinDto>> result = await _communityDragonClient.GetSkinsAsync();
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Count(), Is.EqualTo(1)); 
        Assert.That(result.Value.Single().RiotSkinId, Is.EqualTo(id));
        Assert.That(result.Value.Single().IsBase, Is.EqualTo(isBase));
        Assert.That(result.Value.Single().Name, Is.EqualTo(name));
        Assert.That(result.Value.Single().Description, Is.EqualTo(description));
        Assert.That(result.Value.Single().Rarity, Is.EqualTo(rarity));
        Assert.That(result.Value.Single().IsLegacy, Is.EqualTo(isLegacy));
        Assert.That(result.Value.Single().ChromaPath, Is.EqualTo(chromaPath));
        Assert.That(result.Value.Single().SkinChromaDtos.Count(), Is.EqualTo(1));
        Assert.That(result.Value.Single().SkinChromaDtos.Single().RiotChromaId, Is.EqualTo(chromaId));
        Assert.That(result.Value.Single().SkinChromaDtos.Single().ChromaPath, Is.EqualTo(chromaChromaPath));
        Assert.That(result.Value.Single().SkinChromaDtos.Single().ColorAsStrings, Is.EqualTo(colors));
    }
}