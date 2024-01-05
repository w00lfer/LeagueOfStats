using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Participants.Dtos;

namespace LeagueOfStats.Domain.Matches.Participants;

public class Perks : Entity
{
    private readonly List<PerkStyle> _styles = new();
    
    public Perks(
        AddPerksDto addPerksDto)
        : base(Guid.NewGuid())
    {
        StatPerks = new PerkStats(addPerksDto.AddPerkStatsDto);
        
        _styles.AddRange(addPerksDto.AddPerkStyleDtos
            .Select(addPerkStyleDto => new PerkStyle(addPerkStyleDto)));
    }

    public PerkStats StatPerks { get; }

    public List<PerkStyle> Styles => _styles.ToList();
}