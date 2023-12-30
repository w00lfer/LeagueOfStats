using Camille.RiotGames.Enums;
using LeagueOfStats.Domain.Matches.Teams.Enums;

namespace LeagueOfStats.Application.Extensions;

public static class CamilleTeamExtensions
{
    public static Side ToSide(this Team team) =>
        team switch
        {
            Team.Blue => Side.Blue,
            Team.Red => Side.Red,
            Team.Other => Side.Other,
            _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
        };
}