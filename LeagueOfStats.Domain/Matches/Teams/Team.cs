using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using LeagueOfStats.Domain.Matches.Teams.Enums;

namespace LeagueOfStats.Domain.Matches.Teams;

public class Team : Entity
{
    private readonly List<Ban> _bans = new();

    internal Team(
        AddTeamDto addTeamDto)
        : base(Guid.NewGuid())
    {
        Objectives = new Objectives(addTeamDto.AddObjectivesDto);
        Side = addTeamDto.Side;
        Win = addTeamDto.Win;
        
        _bans.AddRange(addTeamDto.AddBanDtos.Select(addBanDto => new Ban(addBanDto)));
    }
    
    public List<Ban> Bans => _bans.ToList();
    
    public Objectives Objectives { get; }
    
    public Side Side { get; }
    
    public bool Win { get; }
}