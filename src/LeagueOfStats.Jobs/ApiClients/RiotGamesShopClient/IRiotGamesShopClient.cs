using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient;

public interface IRiotGamesShopClient
{
    Task<Result<IEnumerable<RiotGamesShopDiscountDto>>> GetCurrentDiscountsAsync();
}