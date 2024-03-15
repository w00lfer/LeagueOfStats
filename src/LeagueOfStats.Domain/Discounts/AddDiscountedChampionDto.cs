using LeagueOfStats.Domain.Champions;

namespace LeagueOfStats.Domain.Discounts;

public record AddDiscountedChampionDto(
    Champion Champion,
    int OldPrice,
    int NewPrice);