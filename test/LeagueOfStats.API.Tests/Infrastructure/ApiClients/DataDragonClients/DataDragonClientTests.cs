using Bogus;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.API.Infrastructure.ApiClients.DataDragonClient;
using LeagueOfStats.Application.ApiClients.DataDragonClient;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Jobs.ApiClients.DataDragonClient;
using Moq;
using Moq.Contrib.HttpClient;
using NUnit.Framework;

namespace LeagueOfStats.API.Tests.Infrastructure.ApiClients.DataDragonClients;

[TestFixture]
public class DataDragonClientTests
{
    private const string LolGameVersion = "1.0.0";
    private const string HttpClientBaseAddress = "https://ddragon.leagueoflegends.com/";
    private const string VersionsEndpointUrl = "api/versions.json";
    private const string ChampionsEndpointUrl = "cdn/1.0.0/data/en_US/champion.json";
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new ();

    private DataDragonClient _dataDragonClient;

    [SetUp]
    public void SetUp()
    {
        HttpClient httpClient = _httpMessageHandlerMock.CreateClient();
        httpClient.BaseAddress = new Uri(HttpClientBaseAddress);
        
        _dataDragonClient = new(httpClient);
    }

    [Test]
    public async Task GetChampionsAsync_GetVersionsEndpointReturnsNull_ReturnsApiError()
    {
        _httpMessageHandlerMock
            .SetupRequest(HttpMethod.Get, string.Concat(HttpClientBaseAddress, VersionsEndpointUrl))
            .ReturnsJsonResponse<string>(null);

        Result<IEnumerable<ChampionDto>> result = await _dataDragonClient.GetChampionsAsync();
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("Data Dragon versions can't be accessed."));
    } 
    
    [Test]
    public async Task GetChampionsAsync_GetChampionEndpointReturnsNull_ReturnsApiError()
    {
        _httpMessageHandlerMock
            .SetupRequest(HttpMethod.Get, string.Concat(HttpClientBaseAddress, VersionsEndpointUrl))
            .ReturnsJsonResponse(new List<string> { LolGameVersion });
        
        _httpMessageHandlerMock
            .SetupRequest(HttpMethod.Get, string.Concat(HttpClientBaseAddress, ChampionsEndpointUrl))
            .ReturnsJsonResponse<DataDragonChampionDto>(null);

        Result<IEnumerable<ChampionDto>> result = await _dataDragonClient.GetChampionsAsync();
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("Data Dragon champion can't be accessed."));
    } 
    
    [Test]
    public async Task GetChampionsAsync_GetChampionEndpointReturnsChampion_ReturnsResultSuccessWithChampionDtos()
    {
        _httpMessageHandlerMock
            .SetupRequest(HttpMethod.Get, string.Concat(HttpClientBaseAddress, VersionsEndpointUrl))
            .ReturnsJsonResponse(new List<string> { LolGameVersion });

        Faker faker = new();
        string id = faker.Random.Int().ToString();
        string name = faker.Lorem.Word();
        string title = faker.Lorem.Word();
        string description = faker.Lorem.Word();
        Dictionary<string, DataDragonChampionDataDto> dataDragonChampionResponse =
            new Dictionary<string, DataDragonChampionDataDto>()
            {
                {
                    "test", new DataDragonChampionDataDto(
                        id,
                        name,
                        title,
                        description,
                        default)
                }
            };

        DataDragonChampionDto dataDragonChampionDto = new(dataDragonChampionResponse);
        
        _httpMessageHandlerMock
            .SetupRequest(HttpMethod.Get, string.Concat(HttpClientBaseAddress, ChampionsEndpointUrl))
            .ReturnsJsonResponse(dataDragonChampionDto);

        Result<IEnumerable<ChampionDto>> result = await _dataDragonClient.GetChampionsAsync();
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Count(), Is.EqualTo(1));
        Assert.That(result.Value.Single().RiotChampionId, Is.EqualTo(int.Parse(id)));
        Assert.That(result.Value.Single().Name, Is.EqualTo(name));
        Assert.That(result.Value.Single().Title, Is.EqualTo(title));
        Assert.That(result.Value.Single().Description, Is.EqualTo(description));
    }
}