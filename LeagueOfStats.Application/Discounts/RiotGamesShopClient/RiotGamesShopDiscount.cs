using LeagueOfStats.Application.Discounts.Enums;
using NodaTime;

namespace LeagueOfStats.Application.Discounts.RiotGamesShopClient;

public record RiotGamesShopDiscount(
    string Id,
    DiscountType DiscountType,
    int OriginalPrice,
    int DiscountedPrice,
    LocalDateTime SalesStart,
    LocalDateTime SalesEnd);