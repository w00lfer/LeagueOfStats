using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Teams.Dtos;

namespace LeagueOfStats.Domain.Matches.Teams;

public class Objectives : Entity
{
    internal Objectives(
        AddObjectivesDto addObjectivesDto)
        : base(Guid.NewGuid())
    {
        Baron = new Objective(addObjectivesDto.AddBaronObjectiveDto);
        Champion = new Objective(addObjectivesDto.AddChampionObjectiveDto);
        Dragon = new Objective(addObjectivesDto.AddDragonObjectiveDto);
        Horde = addObjectivesDto.AddHordeObjectiveDto is null
            ? null
            : new Objective(addObjectivesDto.AddHordeObjectiveDto);
        Inhibitor = new Objective(addObjectivesDto.AddInhibitorObjectiveDto);
        RiftHerald = new Objective(addObjectivesDto.AddRiftHeraldObjectiveDto);
        Tower = new Objective(addObjectivesDto.AddTowerObjectiveDto);
    }

    public Objective Baron { get; }
    
    public Objective Champion { get; }
    
    public Objective Dragon { get; }
    
    public Objective? Horde { get; }

    public Objective Inhibitor { get; }
    
    public Objective RiftHerald { get; }
    
    public Objective Tower { get; }
}