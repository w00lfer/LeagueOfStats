using LeagueOfStats.Domain.Common.Entities;
using NodaTime;

namespace LeagueOfStats.Domain.Discounts;

public class Discount : AggregateRoot
{
    public Discount(
        LocalDate discountStartDate,
        LocalDate discountEndDate
        )
        : base(Guid.NewGuid())
    {
    }
}