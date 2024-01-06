using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Matches.Teams.Dtos;

namespace LeagueOfStats.Domain.Matches.Teams;

public class Ban : Entity
{
    internal Ban(
        AddBanDto addBanDto,
        Team team)
        : base(Guid.NewGuid())
    {
        Team = team;
        ChampionId = addBanDto.Champion?.Id;
        PickTurn = addBanDto.PickTurn;
    }

    private Ban()
        : base(Guid.Empty)
    {
    }

    public Team Team { get; }

    public Guid? ChampionId { get; }

    public int PickTurn { get; }
}