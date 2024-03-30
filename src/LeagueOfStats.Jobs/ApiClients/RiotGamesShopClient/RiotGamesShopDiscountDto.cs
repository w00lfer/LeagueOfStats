using LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient.Enums;
using NodaTime;

namespace LeagueOfStats.Jobs.ApiClients.RiotGamesShopClient;

public record RiotGamesShopDiscountDto(
    int RiotId,
    DiscountType DiscountType,
    int OriginalPrice,
    int DiscountedPrice,
    LocalDateTime SalesStart,
    LocalDateTime SalesEnd);