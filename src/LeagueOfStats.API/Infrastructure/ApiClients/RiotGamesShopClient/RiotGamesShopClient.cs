using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.Application.ApiClients.RiotGamesShopClient;
using LeagueOfStats.Application.Common.NodaTimeHelpers;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient;
using LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient.Enums;
using NodaTime.Text;

namespace LeagueOfStats.API.Infrastructure.ApiClients.RiotGamesShopClient;

public class RiotGamesShopClient : IRiotGamesShopClient
{
    private readonly HttpClient _httpClient;

    public RiotGamesShopClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<RiotGamesShopDiscountDto>>> GetCurrentDiscountsAsync()
    {
        var riotGamesShopClientResponse = await _httpClient.GetFromJsonAsync<List<RiotGamesShopClientGetCollectionsResponseItemDto>>("collections/");

        if (riotGamesShopClientResponse is null)
        {
            return new ApiError("RiotShopClient can't be accessed.");
        }

        var currentDiscounts = riotGamesShopClientResponse.Where(item => item.Dynamic).ToList();

        var localDateTimePattern = LocalDateTimePatternHelper.RiotLocalDateTimePattern;
        
        var championDiscounts = GetChampionDiscounts(currentDiscounts, localDateTimePattern);
        var skinDiscounts = GetSkinDiscounts(currentDiscounts, localDateTimePattern);

        return Result.Success(championDiscounts.Concat(skinDiscounts));
    }

    private static IEnumerable<RiotGamesShopDiscountDto> GetSkinDiscounts(
        List<RiotGamesShopClientGetCollectionsResponseItemDto> currentDiscounts,
        LocalDateTimePattern localDateTimePattern) =>
        currentDiscounts
            .Select(c => c.DynamicCollection.DiscountedProductsByProductType.ChampionSkin)
            .Where(skinDiscounts => skinDiscounts is not null)
            .SelectMany(skinDiscounts => skinDiscounts)
            .Select(skinDiscount =>
            {
                var price = skinDiscount.Prices.ElementAtOrDefault(0);
                var salesStart = localDateTimePattern.Parse(price.Discount.SaleStart).Value; 
                var salesEnd = localDateTimePattern.Parse(price.Discount.SaleEnd).Value;    
                
                return new RiotGamesShopDiscountDto(
                    int.Parse(skinDiscount.Id),
                    DiscountType.ChampionSkin,
                    price.OriginalPrice.Cost,
                    price.Discount.DiscountedProductPrice.Cost,
                    salesStart,
                    salesEnd);
            })
            .Distinct();

    private static IEnumerable<RiotGamesShopDiscountDto> GetChampionDiscounts(
        List<RiotGamesShopClientGetCollectionsResponseItemDto> currentDiscounts,
        LocalDateTimePattern localDateTimePattern) =>
        currentDiscounts
            .Select(c => c.DynamicCollection.DiscountedProductsByProductType.Champions)
            .Where(championDiscounts => championDiscounts is not null)
            .SelectMany(championDiscounts => championDiscounts)
            .Select(championDiscount =>
            {
                var price = championDiscount.Prices.ElementAtOrDefault(0);
                var salesStart = localDateTimePattern.Parse(price.Discount.SaleStart).Value;
                var salesEnd = localDateTimePattern.Parse(price.Discount.SaleEnd).Value;

                return new RiotGamesShopDiscountDto(
                    int.Parse(championDiscount.Id),
                    DiscountType.Champion,
                    price.OriginalPrice.Cost,
                    price.Discount.DiscountedProductPrice.Cost,
                    salesStart,
                    salesEnd);
            })
            .Distinct();
}