using LeagueOfStats.Domain.Skins;

namespace LeagueOfStats.Domain.Discounts;

public record AddDiscountedSkinDto(
    Skin Skin,
    int OldPrice,
    int NewPrice);