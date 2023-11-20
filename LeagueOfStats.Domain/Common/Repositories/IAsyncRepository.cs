namespace LeagueOfStats.Domain.Common.Repositories
{
    public interface IAsyncRepository<T> : IAsyncReadOnlyRepository<T> where T: class
    {
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
    }
}