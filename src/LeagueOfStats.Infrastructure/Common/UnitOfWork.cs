using System.Data;
using LeagueOfStats.Domain.Common.Repositories;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace LeagueOfStats.Infrastructure.Common;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UnitOfWork(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public IDbTransaction BeginTransaction()
    {
        var transaction = _applicationDbContext.Database.BeginTransaction();

        return transaction.GetDbTransaction();
    }
}