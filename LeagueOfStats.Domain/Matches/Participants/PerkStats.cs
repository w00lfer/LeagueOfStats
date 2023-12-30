using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Participants.Dtos;

namespace LeagueOfStats.Domain.Matches.Participants;

public class PerkStats : Entity
{
    public PerkStats(
        AddPerkStatsDto addPerkStatsDto)
        : base(Guid.NewGuid())
    {
        Defense = addPerkStatsDto.Defense;
        Flex = addPerkStatsDto.Flex;
        Offense = addPerkStatsDto.Offense;
    }

    public int Defense { get; }
    
    public int Flex { get; }
    
    public int Offense { get; }
}