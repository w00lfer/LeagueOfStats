using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Teams.Dtos;

namespace LeagueOfStats.Domain.Matches.Teams;

public class Objectives : Entity
{
    internal Objectives(
        AddObjectivesDto addObjectivesDto,
        Team team)
        : base(Guid.NewGuid())
    {
        Team = team;
        BaronObjective = new Objective(addObjectivesDto.AddBaronObjectiveDto, this);
        ChampionObjective = new Objective(addObjectivesDto.AddChampionObjectiveDto, this);
        DragonObjective = new Objective(addObjectivesDto.AddDragonObjectiveDto, this);
        HordeObjective = addObjectivesDto.AddHordeObjectiveDto is null
            ? null
            : new Objective(addObjectivesDto.AddHordeObjectiveDto, this);
        InhibitorObjective = new Objective(addObjectivesDto.AddInhibitorObjectiveDto, this);
        RiftHeraldObjective = new Objective(addObjectivesDto.AddRiftHeraldObjectiveDto, this);
        TowerObjective = new Objective(addObjectivesDto.AddTowerObjectiveDto, this);
    }

    private Objectives()
        : base(Guid.Empty)
    {
    }

    public Team Team { get; }
    
    public Objective BaronObjective { get; }
    
    public Objective ChampionObjective { get; }
    
    public Objective DragonObjective { get; }
    
    public Objective? HordeObjective { get; }

    public Objective InhibitorObjective { get; }
    
    public Objective RiftHeraldObjective { get; }
    
    public Objective TowerObjective { get; }
}