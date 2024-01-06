using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Participants.Dtos;

namespace LeagueOfStats.Domain.Matches.Participants;

public class PerkStyleSelection : Entity
{
    public PerkStyleSelection(
        AddPerkStyleSelectionDto addPerkStyleSelectionDto,
        PerkStyle perkStyle)
        : base(Guid.NewGuid())
    {
        PerkStyle = perkStyle;
        Perk = addPerkStyleSelectionDto.Perk;
        Var1 = addPerkStyleSelectionDto.Var1;
        Var2 = addPerkStyleSelectionDto.Var2;
        Var3 = addPerkStyleSelectionDto.Var3;
    }

    public PerkStyle PerkStyle { get; }
    
    public int Perk { get; }
    
    public int Var1 { get; }
    
    public int Var2 { get; }
    
    public int Var3 { get; }
}