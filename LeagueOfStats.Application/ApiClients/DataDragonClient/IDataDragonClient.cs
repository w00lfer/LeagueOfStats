using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Application.ApiClients.DataDragonClient;

public interface IDataDragonClient
{
    Task<Result<IEnumerable<ChampionDto>>> GetChampionsAsync();
}