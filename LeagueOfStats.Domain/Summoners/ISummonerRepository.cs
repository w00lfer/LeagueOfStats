using LeagueOfStats.Domain.Common.Repositories;

namespace LeagueOfStats.Domain.Summoners;

public interface ISummonerRepository : IAsyncRepository<Summoner, SummonerId>
{
    Task<Summoner?> GetByPuuid(string puuid);
}