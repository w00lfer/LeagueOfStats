using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Discounts;

public class DiscountedChampion : Entity
{
    public DiscountedChampion(
        Discount discount,
        Champion champion,
        int OldPrice,
        int NewPrice)
        : base(Guid.NewGuid())
    {
    }
}