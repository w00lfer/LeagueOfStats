using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Common.Repositories;

public interface IAsyncRepository<T, TId> : IAsyncReadOnlyRepository<T, TId>
    where T: AggregateRoot<TId> where TId: notnull
{
    Task AddAsync(T entity);
    
    Task DeleteAsync(T entity);
    
    Task UpdateAsync(T entity);
}