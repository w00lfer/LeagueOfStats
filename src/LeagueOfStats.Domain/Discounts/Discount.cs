using LeagueOfStats.Domain.Common.Entities;
using NodaTime;

namespace LeagueOfStats.Domain.Discounts;

public class Discount : AggregateRoot
{
    private readonly List<DiscountedChampion> _discountedChampions = new();
    private readonly List<DiscountedSkin> _discountedSkins = new();
    
    public Discount(AddDiscountDto addDiscountDto)
        : base(Guid.NewGuid())
    {
        StartDateTime = addDiscountDto.StartDateTime;
        EndDateTime = addDiscountDto.EndDateTime;
        
        _discountedChampions.AddRange(addDiscountDto.AddDiscountedChampionDtos
            .Select(addDiscountedChampionDto => new DiscountedChampion(
                this,
                addDiscountedChampionDto.Champion,
                addDiscountedChampionDto.OldPrice,
                addDiscountedChampionDto.NewPrice)));
        
        _discountedSkins.AddRange(addDiscountDto.AddDiscountedSkinDtos
            .Select(addDiscountedSkinDto => new DiscountedSkin(
                this,
                addDiscountedSkinDto.Skin,
                addDiscountedSkinDto.OldPrice,
                addDiscountedSkinDto.NewPrice)));
    }    
    
    protected Discount()
        : base(Guid.Empty)
    {
    }
    
    public LocalDateTime StartDateTime { get; }
    
    public LocalDateTime EndDateTime { get; }

    public List<DiscountedChampion> DiscountedChampions => _discountedChampions.ToList();

    public List<DiscountedSkin> DiscountedSkins => _discountedSkins.ToList();
}