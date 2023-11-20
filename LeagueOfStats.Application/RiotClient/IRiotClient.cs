using Camille.RiotGames;

namespace LeagueOfStats.Application.RiotClient
{
    public interface IRiotClient
    {
        RiotGamesApi GetClient();
    }
}