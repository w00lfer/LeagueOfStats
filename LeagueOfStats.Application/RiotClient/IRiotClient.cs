using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.SummonerV4;
using LanguageExt;
using LeagueOfStats.Domain.Common.Errors;

namespace LeagueOfStats.Application.RiotClient
{
    public interface IRiotClient
    {
        Task<Either<Error, Summoner>> GetSummonerAsync(string server, string summonerName);
        
        Task<Either<Error, ChampionMastery[]>> GetChampionMasteryAsync(string server, string puuid);
    }
}