using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Summoners;

namespace LeagueOfStats.Domain.Common.Repositories;

public interface IAsyncReadOnlyRepository<T>
    where T : AggregateRoot
{
    Task<T?> GetByIdAsync(Guid id);
    
    Task<IEnumerable<T>> GetAllAsync(params Guid[] ids);
}