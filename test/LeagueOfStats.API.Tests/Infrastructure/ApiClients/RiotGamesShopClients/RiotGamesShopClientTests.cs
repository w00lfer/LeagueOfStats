using Bogus;
using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.API.Infrastructure.ApiClients.RiotGamesShopClient;
using LeagueOfStats.Application.ApiClients.RiotGamesShopClient;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient;
using LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient.Enums;
using Moq;
using Moq.Contrib.HttpClient;
using NodaTime;
using NUnit.Framework;

namespace LeagueOfStats.API.Tests.Infrastructure.ApiClients.RiotGamesShopClients;

[TestFixture]
public class RiotGamesShopClientTests
{
    private const string HttpClientBaseAddress = "https://api.shop.riotgames.com/v3/";
    private const string CollectionsEndpointUrl = "collections/";
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new ();

    private RiotGamesShopClient _riotGamesShopClient;

    [SetUp]
    public void SetUp()
    {
        HttpClient httpClient = _httpMessageHandlerMock.CreateClient();
        httpClient.BaseAddress = new Uri(HttpClientBaseAddress);
        
        _riotGamesShopClient = new(httpClient);
    }

    [Test]
    public async Task GetCurrentDiscountsAsync_GetCollectionsEndpointReturnsNull_ReturnsApiError()
    {
        _httpMessageHandlerMock
            .SetupRequest(HttpMethod.Get, string.Concat(HttpClientBaseAddress, CollectionsEndpointUrl))
            .ReturnsJsonResponse<List<RiotGamesShopClientGetCollectionsResponseItemDto>>(null);

        Result<IEnumerable<RiotGamesShopDiscountDto>> result = await _riotGamesShopClient.GetCurrentDiscountsAsync();
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Single(), Is.TypeOf<ApiError>());
        Assert.That(result.Errors.Single().ErrorMessage, Is.EqualTo("RiotShopClient can't be accessed."));
    } 
    
    [Test]
    public async Task GetCurrentDiscountsAsync_GetCollectionsEndpointReturnsCollections_ReturnsApiError()
    {
        Faker faker = new();

        RiotGamesShopClientDiscountedChampion riotGamesShopClientDiscountedSkin = GetRiotGamesShopClientDiscountedChampionSkin(faker);
        RiotGamesShopClientDiscountedChampion riotGamesShopClientDiscountedChampion = GetRiotGamesShopClientDiscountedChampion(faker);
        RiotGamesShopClientGetCollectionsResponseItemDto riotGamesShopClientGetCollectionsResponseItemDto = new(
            Products: default,
            Path: default,
            Dynamic: true,
            DynamicCollection: new RiotGamesShopClientDynamicCollection(
                LatestProducts: default,
                DiscountedProductsByProductType: new RiotGamesShopClientDiscountedProductsByProductType(
                    ChampionSkin: new List<RiotGamesShopClientDiscountedChampion>()
                    {
                        riotGamesShopClientDiscountedSkin
                    },
                    Champions: new List<RiotGamesShopClientDiscountedChampion>()
                    {
                        riotGamesShopClientDiscountedChampion
                    })
            ),
            Empty: default,
            BannerUrl: default,
            BannerTitle: default,
            LogoUrl: default
        );

        List<RiotGamesShopClientGetCollectionsResponseItemDto> riotGamesShopClientGetCollectionsResponseItemDtos = new()
        {
            riotGamesShopClientGetCollectionsResponseItemDto
        };
        
        _httpMessageHandlerMock
            .SetupRequest(HttpMethod.Get, string.Concat(HttpClientBaseAddress, CollectionsEndpointUrl))
            .ReturnsJsonResponse(riotGamesShopClientGetCollectionsResponseItemDtos);

        Result<IEnumerable<RiotGamesShopDiscountDto>> result = await _riotGamesShopClient.GetCurrentDiscountsAsync();
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Count(), Is.EqualTo(2));

        RiotGamesShopDiscountDto discountedSkin = result.Value.Single(d => d.DiscountType == DiscountType.ChampionSkin);
        Assert.That(discountedSkin.RiotId, Is.EqualTo(int.Parse(riotGamesShopClientDiscountedSkin.Id)));
        Assert.That(discountedSkin.OriginalPrice, Is.EqualTo(riotGamesShopClientDiscountedSkin.Prices.Single().OriginalPrice.Cost));
        Assert.That(discountedSkin.DiscountedPrice, Is.EqualTo(riotGamesShopClientDiscountedSkin.Prices.Single().Discount.DiscountedProductPrice.Cost));
        Assert.That(discountedSkin.SalesStart, Is.EqualTo(new LocalDateTime(2024, 3, 22, 12, 0, 0)));
        Assert.That(discountedSkin.SalesEnd, Is.EqualTo(new LocalDateTime(2024, 3, 29, 12, 0, 0)));
        
        RiotGamesShopDiscountDto discountedChampion = result.Value.Single(d => d.DiscountType == DiscountType.Champion);
        Assert.That(discountedChampion.RiotId, Is.EqualTo(int.Parse(riotGamesShopClientDiscountedChampion.Id)));
        Assert.That(discountedChampion.OriginalPrice, Is.EqualTo(riotGamesShopClientDiscountedChampion.Prices.Single().OriginalPrice.Cost));
        Assert.That(discountedChampion.DiscountedPrice, Is.EqualTo(riotGamesShopClientDiscountedChampion.Prices.Single().Discount.DiscountedProductPrice.Cost));
        Assert.That(discountedChampion.SalesStart, Is.EqualTo(new LocalDateTime(2024, 3, 22, 12, 0, 0)));
        Assert.That(discountedChampion.SalesEnd, Is.EqualTo(new LocalDateTime(2024, 3, 29, 12, 0, 0)));
    }

    private RiotGamesShopClientDiscountedChampion GetRiotGamesShopClientDiscountedChampionSkin(Faker faker)
    {
        string championSkinId = faker.Random.Int().ToString();
        int championSkinOriginalPrice = faker.Random.Int(min: 400, max: 1800);
        int championSkinDiscountedPrice = faker.Random.Int(min: 1, max: 399);
        const string championSkinDiscountStart = "2024-03-22-12-00-00";
        const string championSkinDiscountEnd = "2024-03-29-12-00-00";
        RiotGamesShopClientDiscountedChampion discountedChampionSkin = new(
            Game: default,
            Type: default,
            Id: championSkinId,
            Status: default,
            Prices: new List<RiotGamesShopClientPrice>()
            {
                new(
                    Type: default,
                    OriginalPrice: new RiotGamesShopClientOriginalPrice(
                        Currency: default,
                        Cost: championSkinOriginalPrice),
                    FinalPrice: default,
                    Discount: new RiotGamesShopClientDiscount(
                        DiscountedProductPrice: new RiotGamesShopClientDiscountedProductPrice(
                            Currency: default,
                            Cost: championSkinDiscountedPrice),
                        SaleStart: championSkinDiscountStart,
                        SaleEnd: championSkinDiscountEnd))
            },
            Contents: default
        );

        return discountedChampionSkin;
    }
    
    private RiotGamesShopClientDiscountedChampion GetRiotGamesShopClientDiscountedChampion(Faker faker)
    {
        string championId = faker.Random.Int().ToString();
        int championOriginalPrice = faker.Random.Int(min: 400, max: 1800);
        int championDiscountedPrice = faker.Random.Int(min: 1, max: 399);
        const string championDiscountStart = "2024-03-22-12-00-00";
        const string championDiscountEnd = "2024-03-29-12-00-00";
        RiotGamesShopClientDiscountedChampion discountedChampion = new(
            Game: default,
            Type: default,
            Id: championId,
            Status: default,
            Prices: new List<RiotGamesShopClientPrice>()
            {
                new(
                    Type: default,
                    OriginalPrice: new RiotGamesShopClientOriginalPrice(
                        Currency: default,
                        Cost: championOriginalPrice),
                    FinalPrice: default,
                    Discount: new RiotGamesShopClientDiscount(
                        DiscountedProductPrice: new RiotGamesShopClientDiscountedProductPrice(
                            Currency: default,
                            Cost: championDiscountedPrice),
                        SaleStart: championDiscountStart,
                        SaleEnd: championDiscountEnd))
            },
            Contents: default
        );

        return discountedChampion;
    }
}