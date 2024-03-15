using LeagueOfStats.Domain.Common.Entities;
using LeagueOfStats.Domain.Common.Repositories;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.Common;

public abstract class AsyncRepositoryBase<T> : IAsyncRepository<T> where T : AggregateRoot
{
    protected readonly ApplicationDbContext _applicationDbContext;

    protected AsyncRepositoryBase(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    public virtual Task<T?> GetByIdAsync(Guid id) => 
        _applicationDbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

    public virtual async Task<IEnumerable<T>> GetAllAsync(params Guid[] ids) =>
        ids.Length > 0
            ? await _applicationDbContext.Set<T>().Where(c => ids.Contains(c.Id)).AsNoTracking().ToListAsync()
            : await _applicationDbContext.Set<T>().AsNoTracking().ToListAsync();

    public virtual async Task AddAsync(T entity) => 
        await _applicationDbContext.Set<T>().AddAsync(entity);

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _applicationDbContext.Set<T>().AddRangeAsync(entities);

        await _applicationDbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _applicationDbContext.Set<T>().Remove(entity);

        await _applicationDbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _applicationDbContext.Set<T>().Update(entity);

        await _applicationDbContext.SaveChangesAsync();
    }
}