using LeagueOfStats.Application.RiotGamesShopClient.Enums;

namespace LeagueOfStats.Application.RiotGamesShopClient;

public record DiscountedProductDto(
    DiscountType DiscountType,
    string ProductId,
    int OldPrice,
    int NewPrice);