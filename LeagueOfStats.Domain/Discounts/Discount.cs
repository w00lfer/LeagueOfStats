using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Discounts;

public class Discount : AggregateRoot
{
    public Discount()
        : base(Guid.NewGuid())
    {
    }
}