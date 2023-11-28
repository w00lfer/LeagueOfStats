using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Common.Repositories;

public interface IAsyncRepository<T> : IAsyncReadOnlyRepository<T> where T: Entity, IAggregateRoot
{
    Task AddAsync(T entity);
    Task DeleteAsync(T entity);
}