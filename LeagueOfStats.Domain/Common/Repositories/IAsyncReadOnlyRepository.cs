using LeagueOfStats.Domain.Common.Entities;

namespace LeagueOfStats.Domain.Common.Repositories;

public interface IAsyncReadOnlyRepository<T> where T : Entity, IAggregateRoot
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync(params int[] ids);
}