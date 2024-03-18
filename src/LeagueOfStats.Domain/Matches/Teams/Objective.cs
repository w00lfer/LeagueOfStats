using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Teams.Dtos;

namespace LeagueOfStats.Domain.Matches.Teams;

public class Objective : Entity
{
    internal Objective(
        AddObjectiveDto addObjectiveDto,
        Objectives objectives)
        : base(Guid.NewGuid())
    {
        Objectives = objectives;
        First = addObjectiveDto.First;
        Kills = addObjectiveDto.Kills;
    }

    protected Objective()
        : base(Guid.Empty)
    {
    }

    public Objectives Objectives { get; }

    public bool First { get; }

    public int Kills { get; }
}