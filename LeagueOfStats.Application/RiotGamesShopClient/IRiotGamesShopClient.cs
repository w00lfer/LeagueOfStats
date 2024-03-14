using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Application.RiotGamesShopClient;

public interface IRiotGamesShopClient
{
    Task<Result<IEnumerable<RiotGamesShopDiscount>>> GetCurrentDiscountsAsync();
}