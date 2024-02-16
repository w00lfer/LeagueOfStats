using NodaTime;

namespace LeagueOfStats.Application.Discounts.RiotGamesShopClient;

public record RiotGamesShopDiscount(
    string Type,
    string Id,
    int OriginalPrice,
    int DiscountedPrice,
    LocalDateTime SalesStart,
    LocalDateTime SalesEnd);