using LeagueOfStats.Application.ApiClients.RiotGamesShopClient.Enums;

namespace LeagueOfStats.Application.ApiClients.RiotGamesShopClient;

public record DiscountedProductDto(
    DiscountType DiscountType,
    string ProductId,
    int OldPrice,
    int NewPrice);