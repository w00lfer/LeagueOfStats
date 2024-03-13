using NodaTime;

namespace LeagueOfStats.Domain.Discounts;

public record AddDiscountDto(
    LocalDateTime StartDateTime,
    LocalDateTime EndDateTime,
    IEnumerable<AddDiscountedChampionDto> AddDiscountedChampionDtos,
    IEnumerable<AddDiscountedSkinDto> AddDiscountedSkinDtos);