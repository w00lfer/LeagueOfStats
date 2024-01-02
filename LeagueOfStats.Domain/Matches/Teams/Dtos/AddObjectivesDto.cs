namespace LeagueOfStats.Domain.Matches.Teams.Dtos;

public record AddObjectivesDto(
    AddObjectiveDto AddBaronObjectiveDto,
    AddObjectiveDto AddChampionObjectiveDto,
    AddObjectiveDto AddDragonObjectiveDto,
    AddObjectiveDto? AddHordeObjectiveDto,
    AddObjectiveDto AddInhibitorObjectiveDto,
    AddObjectiveDto AddRiftHeraldObjectiveDto,
    AddObjectiveDto AddTowerObjectiveDto);