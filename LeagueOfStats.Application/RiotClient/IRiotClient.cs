using Camille.RiotGames.ChampionMasteryV4;
using Camille.RiotGames.SummonerV4;
using LanguageExt;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Errors;

namespace LeagueOfStats.Application.RiotClient;

public interface IRiotClient
{
    Task<Either<Error, Summoner>> GetSummonerByPuuidAsync(string puuid, Region region);

    Task<Either<Error, Summoner>> GetSummonerByGameNameAndTaglineAsync(string gameName, string tagLine, Region region);
        
    Task<Either<Error, ChampionMastery[]>> GetSummonerChampionMasteryByPuuid(string puuid, Region region);
}