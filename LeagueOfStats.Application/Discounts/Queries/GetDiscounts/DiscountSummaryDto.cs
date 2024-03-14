using NodaTime;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscounts;

public record DiscountSummaryDto(
    Guid Id,
    LocalDateTime StartDateTime,
    LocalDateTime EndDateTime);