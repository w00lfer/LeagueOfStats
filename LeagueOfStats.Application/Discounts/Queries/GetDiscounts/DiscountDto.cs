using NodaTime;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscounts;

public record DiscountDto(
    Guid Id,
    LocalDate StartDate,
    LocalDate EndDate);