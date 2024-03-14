using LeagueOfStats.Application.ApiClients.RiotGamesShopClient.Enums;
using NodaTime;

namespace LeagueOfStats.Application.ApiClients.RiotGamesShopClient;

public record RiotGamesShopDiscount(
    string Id,
    DiscountType DiscountType,
    int OriginalPrice,
    int DiscountedPrice,
    LocalDateTime SalesStart,
    LocalDateTime SalesEnd);