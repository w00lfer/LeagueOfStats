using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Participants.Dtos;

namespace LeagueOfStats.Domain.Matches.Participants;

public class PerkStyle : Entity
{
    private readonly List<PerkStyleSelection> _selections = new();
    
    public PerkStyle(
        AddPerkStyleDto addPerkStyleDto,
        Perks perks)
        : base(Guid.NewGuid())
    {
        Perks = perks;
        Description = addPerkStyleDto.Description;
        Style = addPerkStyleDto.Style;
        
        _selections.AddRange(addPerkStyleDto.AddPerkStyleSelectionDtos
            .Select(addPerkStyleSelectionDto => new PerkStyleSelection(addPerkStyleSelectionDto, this)));
    }
    
    public Perks Perks { get; }
    
    public string Description { get; }

    public List<PerkStyleSelection> Selections => _selections.ToList();
    
    public int Style { get; }
}