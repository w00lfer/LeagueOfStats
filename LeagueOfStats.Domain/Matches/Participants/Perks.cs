using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Participants.Dtos;

namespace LeagueOfStats.Domain.Matches.Participants;

public class Perks : Entity
{
    private readonly List<PerkStyle> _styles = new();

    public Perks(
        AddPerksDto addPerksDto,
        Participant participant)
        : base(Guid.NewGuid())
    {
        Participant = participant;
        StatPerks = new PerkStats(addPerksDto.AddPerkStatsDto, this);

        _styles.AddRange(addPerksDto.AddPerkStyleDtos
            .Select(addPerkStyleDto => new PerkStyle(addPerkStyleDto, this)));
    }

    public Participant Participant { get; }

    public PerkStats StatPerks { get; }

    public List<PerkStyle> Styles => _styles.ToList();
}