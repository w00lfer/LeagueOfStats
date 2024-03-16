namespace LeagueOfStats.Application.Discounts.Queries.GetDiscountById.Dtos;

public record DiscountedChampionDto(
    string Name,
    string FullFileName,
    decimal OldPrice,
    decimal NewPrice);