using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Jobs.ApiClients.DataDragonClient;

public interface IDataDragonClient
{
    Task<Result<IEnumerable<ChampionDto>>> GetChampionsAsync();
}