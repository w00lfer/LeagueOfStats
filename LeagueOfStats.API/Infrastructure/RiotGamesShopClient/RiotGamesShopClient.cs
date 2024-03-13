using LeagueOfStats.API.Common.Errors;
using LeagueOfStats.Application.Common.NodaTimeHelpers;
using LeagueOfStats.Application.Discounts.Enums;
using LeagueOfStats.Application.Discounts.RiotGamesShopClient;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.API.Infrastructure.RiotGamesShopClient;

public class RiotGamesShopClient : IRiotGamesShopClient
{
    private readonly HttpClient _httpClient;

    public RiotGamesShopClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<RiotGamesShopDiscount>>> GetCurrentDiscountsAsync()
    {
        var response1 = await _httpClient.GetFromJsonAsync<List<RiotGamesShopClientGetCollectionsResponseItemDto>>("collections/");

        if (response1 is null)
        {
            return new ApiError("");
        }

        var currentDiscounts = response1.Where(item => item.Dynamic).ToList();

        var localDateTimePattern = LocalDateTimePatternHelper.RiotLocalDateTimePattern;
        
        var championDiscounts = currentDiscounts
            .Select(c => c.DynamicCollection.DiscountedProductsByProductType.Champions)
            .Where(championDiscounts => championDiscounts is not null)
            .SelectMany(championDiscounts => championDiscounts)
            .Select(championDiscount =>
            {
                var price = championDiscount.Prices.ElementAtOrDefault(0);
                var salesStart = localDateTimePattern.Parse(price.Discount.SaleStart).Value;
                var salesEnd = localDateTimePattern.Parse(price.Discount.SaleEnd).Value;    
                
                return new RiotGamesShopDiscount(
                    championDiscount.Id,
                    DiscountType.Champion,
                    price.OriginalPrice.Cost,
                    price.Discount.DiscountedProductPrice.Cost,
                    salesStart,
                    salesEnd);
            });

        var skinDiscounts = currentDiscounts
            .Select(c => c.DynamicCollection.DiscountedProductsByProductType.ChampionSkin)
            .Where(skinDiscounts => skinDiscounts is not null)
            .SelectMany(skinDiscounts => skinDiscounts)
            .Select(skinDiscount =>
            {
                var price = skinDiscount.Prices.ElementAtOrDefault(0);
                var salesStart = localDateTimePattern.Parse(price.Discount.SaleStart).Value; 
                var salesEnd = localDateTimePattern.Parse(price.Discount.SaleEnd).Value;    
                
                return new RiotGamesShopDiscount(
                    skinDiscount.Id,
                    DiscountType.ChampionSkin,
                    price.OriginalPrice.Cost,
                    price.Discount.DiscountedProductPrice.Cost,
                    salesStart,
                    salesEnd);
            });

        return Result.Success(championDiscounts.Concat(skinDiscounts));
    }
}