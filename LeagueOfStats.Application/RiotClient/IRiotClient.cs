using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.SummonerV4;
using CSharpFunctionalExtensions;

namespace LeagueOfStats.Application.RiotClient
{
    public interface IRiotClient
    {
        Task<Maybe<Summoner>> GetSummonerAsync(string server, string summonerName);
        
        Task<Maybe<ChampionMastery[]>> GetChampionMasteryAsync(string server, string puuid);
    }
}