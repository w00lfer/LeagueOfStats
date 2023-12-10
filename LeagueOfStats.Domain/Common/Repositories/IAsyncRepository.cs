using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Common.Repositories;

public interface IAsyncRepository<T> : IAsyncReadOnlyRepository<T>
    where T: AggregateRoot
{
    Task AddAsync(T entity);
    
    Task DeleteAsync(T entity);
    
    Task UpdateAsync(T entity);
}