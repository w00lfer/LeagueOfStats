using LeagueOfStats.Domain.Common.Repositories;

namespace LeagueOfStats.Domain.Summoners;

public interface ISummonerRepository : IAsyncRepository<Summoner>
{
    Task<Summoner?> GetByIdWithAllIncludesAsync(Guid id);
    
    Task<Summoner?> GetByPuuidAsync(string puuid);

    Task<IEnumerable<Summoner>> GetByPuuidsAsync(IEnumerable<string> puuids);
}