using System.Text.Json.Serialization;

namespace LeagueOfStats.Application.ApiClients.RiotGamesShopClient;

public record RiotGamesShopClientDiscountedChampion(
    string Game,
    string Type,
    string Id,
    string Status,
    List<RiotGamesShopClientPrice> Prices,
    List<object> Contents
);

public record RiotGamesShopClientDiscountChampionSkin(
    string Game,
    string Type,
    string Id,
    string Status,
    List<RiotGamesShopClientPrice> Prices,
    List<object> Contents
);

public record RiotGamesShopClientDiscount(
    RiotGamesShopClientDiscountedProductPrice DiscountedProductPrice,
    string SaleStart,
    string SaleEnd
);

public record RiotGamesShopClientDiscountedProductPrice(
    string Currency,
    int Cost
);

public record RiotGamesShopClientDiscountedProductsByProductType(
    [property: JsonPropertyName("CHAMPION_SKIN")] List<RiotGamesShopClientDiscountedChampion>? ChampionSkin,
    [property: JsonPropertyName("CHAMPION")]List<RiotGamesShopClientDiscountedChampion>? Champions
);

public record RiotGamesShopClientDynamicCollection(
    List<RiotGamesShopClientLatestProduct> LatestProducts,
    RiotGamesShopClientDiscountedProductsByProductType DiscountedProductsByProductType
);

public record RiotGamesShopClientFinalPrice(
    string Currency,
    int Cost
);

public record RiotGamesShopClientLatestProduct(
    string Game,
    string Type,
    string Id,
    string Status,
    List<RiotGamesShopClientPrice> Prices,
    List<object> Contents
);

public record RiotGamesShopClientOriginalPrice(
    string Currency,
    int Cost
);

public record RiotGamesShopClientPrice(
    string Type,
    RiotGamesShopClientOriginalPrice OriginalPrice,
    RiotGamesShopClientFinalPrice FinalPrice,
    RiotGamesShopClientDiscount Discount
);

public record RiotGamesShopClientProduct(
    string Game,
    string Type,
    string Id,
    string Status,
    List<RiotGamesShopClientPrice> Prices,
    List<object> Contents
);

public record RiotGamesShopClientGetCollectionsResponseItemDto(
    List<RiotGamesShopClientProduct> Products,
    string Path,
    bool Dynamic,
    RiotGamesShopClientDynamicCollection DynamicCollection,
    bool Empty,
    string BannerUrl,
    string BannerTitle,
    string LogoUrl
);