namespace LeagueOfStats.Domain.Common.Repositories
{
    public interface IAsyncReadOnlyRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}