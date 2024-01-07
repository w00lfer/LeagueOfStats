using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Participants.Dtos;

namespace LeagueOfStats.Domain.Matches.Participants;

public class PerkStats : Entity
{
    public PerkStats(
        AddPerkStatsDto addPerkStatsDto,
        Perks perks)
        : base(Guid.NewGuid())
    {
        Perks = perks;
        Defense = addPerkStatsDto.Defense;
        Flex = addPerkStatsDto.Flex;
        Offense = addPerkStatsDto.Offense;
    }

    private PerkStats()
        : base(Guid.Empty)
    {
    }

    public Perks Perks { get; }

    public int Defense { get; }

    public int Flex { get; }

    public int Offense { get; }
}