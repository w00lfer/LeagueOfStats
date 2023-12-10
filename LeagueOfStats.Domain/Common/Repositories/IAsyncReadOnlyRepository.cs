using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Common.Repositories;

public interface IAsyncReadOnlyRepository<T>
    where T : AggregateRoot
{
    Task<T?> GetByIdAsync(Guid id);
    
    Task<IEnumerable<T>> GetAllAsync(params Guid[] ids);
}