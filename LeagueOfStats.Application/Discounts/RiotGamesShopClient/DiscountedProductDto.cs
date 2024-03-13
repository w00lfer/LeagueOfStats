using LeagueOfStats.Application.Discounts.Enums;

namespace LeagueOfStats.Application.Discounts.RiotGamesShopClient;

public record DiscountedProductDto(
    DiscountType DiscountType,
    string ProductId,
    int OldPrice,
    int NewPrice);