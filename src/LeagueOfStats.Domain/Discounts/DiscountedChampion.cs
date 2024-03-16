using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Discounts;

public class DiscountedChampion : Entity
{
    public DiscountedChampion(
        Discount discount,
        Champion champion,
        int oldPrice,
        int newPrice)
        : base(Guid.NewGuid())
    {
        Discount = discount;
        ChampionId = champion.Id;
        OldPrice = oldPrice;
        NewPrice = newPrice;
    }
    
    private DiscountedChampion()
        : base(Guid.Empty)
    {
    }
    
    public Discount Discount { get; }
    
    public Guid ChampionId { get; }
    
    public int OldPrice { get; }
    
    public int NewPrice { get; }
}