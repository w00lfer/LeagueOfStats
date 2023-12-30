using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Teams.Dtos;

namespace LeagueOfStats.Domain.Matches.Teams;

public class Objective : Entity
{
    internal Objective(
        AddObjectiveDto addObjectiveDto)
        : base(Guid.NewGuid())
    {
        First = addObjectiveDto.First;
        Kills = addObjectiveDto.Kills;
    }

    public bool First { get; }

    public int Kills { get; }

}