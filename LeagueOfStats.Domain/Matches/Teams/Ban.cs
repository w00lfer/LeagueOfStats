using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Teams.Dtos;

namespace LeagueOfStats.Domain.Matches.Teams;

public class Ban : Entity
{
    internal Ban(
        AddBanDto addBanDto) 
        : base(Guid.NewGuid())
    {
        ChampionId = addBanDto.Champion?.Id;
        PickTurn = addBanDto.PickTurn;
    }

    public Guid? ChampionId { get; }
    
    public int PickTurn { get; }
}