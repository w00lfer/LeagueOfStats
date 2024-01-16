namespace LeagueOfStats.Application.Discounts.RiotGamesShopClient;

public record GetCollectionsResponseDto(
    GetCollectionsResponseItemDto[] Collections);

public record GetCollectionsResponseItemDto(
    string Path,
    bool Dynamic,
    DynamicCollectionDto DynamicCollection);
    
public record DynamicCollectionDto(
    Dictionary<string, ProductDto> DiscountedProductsByProductType);
    
public record ProductDto(
    string Game,
    string Type,
    string Id,
    PriceDto Prices);

public record PriceDto(
    PriceCostDto OriginalPrice,
    PriceDiscountDto Discount,
    PriceCostDto FinalPrice);

public record PriceDiscountDto(
    PriceCostDto DiscountedProductPrice,
    string SalesStart,
    string SalesEnd);

public record PriceCostDto(
    int Cost);