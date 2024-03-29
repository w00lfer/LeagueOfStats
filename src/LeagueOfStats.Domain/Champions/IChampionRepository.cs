using LeagueOfStats.Domain.Common.Repositories;

namespace LeagueOfStats.Domain.Champions;

public interface IChampionRepository : IAsyncRepository<Champion>
{
}