using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Application.Discounts.RiotGamesShopClient;

public interface IRiotGamesShopClient
{
    Task<Result<IEnumerable<ProductDto>>> GetCurrentDiscountsAsync();
}