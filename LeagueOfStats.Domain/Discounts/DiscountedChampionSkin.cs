using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Discounts;

public class DiscountedChampionSkin : Entity
{
    public DiscountedChampionSkin(
        Discount discount,
        Champion champion,
        int oldPrice,
        int newPrice,
        string name,
        string imagePath)
        : base(Guid.NewGuid())
    {
    }
}