namespace LeagueOfStats.Domain.Common.Repositories
{
    public interface IReadOnlyRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
    }
}