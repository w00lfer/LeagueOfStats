using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Discounts;

public class DiscountedChampionSkin : Entity
{
    public DiscountedChampionSkin(
        Discount discount,
        Champion champion,
        int OldPrice,
        int NewPrice)
        : base(Guid.NewGuid())
    {
    }
}