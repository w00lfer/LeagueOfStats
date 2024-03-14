using LeagueOfStats.Application.RiotGamesShopClient.Enums;
using NodaTime;

namespace LeagueOfStats.Application.RiotGamesShopClient;

public record RiotGamesShopDiscount(
    string Id,
    DiscountType DiscountType,
    int OriginalPrice,
    int DiscountedPrice,
    LocalDateTime SalesStart,
    LocalDateTime SalesEnd);