namespace LeagueOfStats.Application.Discounts.Queries.GetDiscountById.Dtos;

public record DiscountedSkinDto(
    string Name,
    string FullFileName,
    string Rarity,
    decimal OldPrice,
    decimal NewPrice);