using LanguageExt;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Errors;
using LeagueOfStats.Domain.Summoners;

namespace LeagueOfStats.Application.Summoners;

public interface ISummonerApplicationService
{
    Task<Either<Error, Summoner>> GetSummonerByGameNameAndTagLineAndRegion(string gameName, string tagLine, Region region);

    Task<Either<Error, Summoner>> GetSummonerById(Guid id);

    Task<Either<Error, IEnumerable<SummonerChampionMastery>>> GetSummonerChampionMasteriesBySummonerId(Guid summonerId);
}