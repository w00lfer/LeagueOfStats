using System.Text.Json.Serialization;

public record DiscountedChampion(
    string Game,
    string Type,
    string Id,
    string Status,
    List<Price> Prices,
    List<object> Contents
);

public record DiscountChampionSkin(
    string Game,
    string Type,
    string Id,
    string Status,
    List<Price> Prices,
    List<object> Contents
);

public record Discount(
    DiscountedProductPrice DiscountedProductPrice,
    string SaleStart,
    string SaleEnd
);

public record DiscountedProductPrice(
    string Currency,
    int Cost
);

public record DiscountedProductsByProductType(
    [property: JsonPropertyName("CHAMPION_SKIN")] List<DiscountedChampion>? ChampionSkin,
    [property: JsonPropertyName("CHAMPION")]List<DiscountedChampion>? Champions
);

public record DynamicCollection(
    List<LatestProduct> LatestProducts,
    DiscountedProductsByProductType DiscountedProductsByProductType
);

public record FinalPrice(
    string Currency,
    int Cost
);

public record LatestProduct(
    string Game,
    string Type,
    string Id,
    string Status,
    List<Price> Prices,
    List<object> Contents
);

public record OriginalPrice(
    string Currency,
    int Cost
);

public record Price(
    string Type,
    OriginalPrice OriginalPrice,
    FinalPrice FinalPrice,
    Discount Discount
);

public record Product(
    string Game,
    string Type,
    string Id,
    string Status,
    List<Price> Prices,
    List<object> Contents
);

public record GetCollectionsResponseItemDto(
    List<Product> Products,
    string Path,
    bool Dynamic,
    DynamicCollection DynamicCollection,
    bool Empty,
    string BannerUrl,
    string BannerTitle,
    string LogoUrl
);