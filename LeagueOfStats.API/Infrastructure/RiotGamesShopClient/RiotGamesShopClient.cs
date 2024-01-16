using LeagueOfStats.API.Common.Errors;
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

    public async Task<Result<IEnumerable<ProductDto>>> GetCurrentDiscountsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<GetCollectionsResponseDto>("collections/");

        if (response is null)
        {
            return new ApiError("");
        }

        var currentDiscounts = response.Collections.Where(c => c.Dynamic).ToList();

        var championDiscounts = currentDiscounts.Select(c =>
            c.DynamicCollection.DiscountedProductsByProductType.Single(kv => kv.Key is "CHAMPION").Value);

        var skinDiscounts = currentDiscounts.Select(c =>
            c.DynamicCollection.DiscountedProductsByProductType.Single(kv => kv.Key is "CHAMPION_SKIN").Value);

        return Result.Success(championDiscounts.Concat(skinDiscounts).DistinctBy(p => new { p.Type, p.Id }));
    }
}