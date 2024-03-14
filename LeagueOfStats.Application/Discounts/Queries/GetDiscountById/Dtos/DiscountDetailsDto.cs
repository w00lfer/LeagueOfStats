using NodaTime;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscountById.Dtos;

public record DiscountDetailsDto(
    LocalDateTime StartDateTime,
    LocalDateTime EndDateTime,
    IEnumerable<DiscountedChampionDto> DiscountedChampionDtos,
    IEnumerable<DiscountedSkinDto> DiscountedSkinDtos);