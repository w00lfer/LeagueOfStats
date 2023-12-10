using LeagueOfStats.Domain.Summoners;

namespace LeagueOfStats.Infrastructure.Summoners;

public class SummonerRepository : ISummonerRepository
{
    private List<Summoner> _summoners = new();

    public Task<Summoner?> GetByIdAsync(Guid id) =>
        Task.FromResult(_summoners.SingleOrDefault(s => s.Id == id));

    public Task<Summoner?> GetByPuuid(string puuid) =>
        Task.FromResult(_summoners.SingleOrDefault(s => s.Puuid == puuid));
    
    public Task<IEnumerable<Summoner>> GetAllAsync(params Guid[] ids) =>
        Task.FromResult(ids.Length > 0
            ? _summoners.Where(s => ids.Contains(s.Id))
            : _summoners);

    public Task AddAsync(Summoner entity)
    {
        _summoners.Add(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Summoner entity)
    {
        _summoners.Remove(entity);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(Summoner entity)
    {
        // As it' in memory stored once smbd updates entity it should be done automatically
        return Task.CompletedTask;
    }
}