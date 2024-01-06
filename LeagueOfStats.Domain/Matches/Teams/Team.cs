using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using LeagueOfStats.Domain.Matches.Teams.Enums;

namespace LeagueOfStats.Domain.Matches.Teams;

public class Team : Entity
{
    private readonly List<Ban> _bans = new();

    internal Team(
        AddTeamDto addTeamDto,
        Match match)
        : base(Guid.NewGuid())
    {
        Match = match;
        Objectives = new Objectives(addTeamDto.AddObjectivesDto, this);
        Side = addTeamDto.Side;
        Win = addTeamDto.Win;
        
        _bans.AddRange(addTeamDto.AddBanDtos.Select(addBanDto => new Ban(addBanDto)));
    }
    
    public Match Match { get; }
    
    public List<Ban> Bans => _bans.ToList();
    
    public Objectives Objectives { get; }
    
    public Side Side { get; }
    
    public bool Win { get; }
}