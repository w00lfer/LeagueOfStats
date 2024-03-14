using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Skins;

namespace LeagueOfStats.Domain.Discounts;

public class DiscountedSkin : Entity
{
    public DiscountedSkin(
        Discount discount,
        Skin skin,
        int oldPrice,
        int newPrice)
        : base(Guid.NewGuid())
    {
        Discount = discount;
        SkinId = skin.Id;
        OldPrice = oldPrice;
        NewPrice = newPrice;
    }
    
    private DiscountedSkin()
        : base(Guid.Empty)
    {
    }
    
    public Discount Discount { get; }
    
    public Guid SkinId { get; }
    
    public int OldPrice { get; }
    
    public int NewPrice { get; }
}