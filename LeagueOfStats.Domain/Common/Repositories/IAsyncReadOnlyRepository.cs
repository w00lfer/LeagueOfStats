using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Common.Repositories;

public interface IAsyncReadOnlyRepository<T, TId>
    where T : AggregateRoot<TId> 
    where TId: notnull
{
    Task<T?> GetByIdAsync(TId id);
    
    Task<IEnumerable<T>> GetAllAsync(params TId[] ids);
}