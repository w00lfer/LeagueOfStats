using LeagueOfStats.Domain.Common.Repositories;

namespace LeagueOfStats.Domain.Summoners;

public interface ISummonerRepository : IAsyncRepository<Summoner>
{
    Task<Summoner?> GetByPuuidAsync(string puuid);
}