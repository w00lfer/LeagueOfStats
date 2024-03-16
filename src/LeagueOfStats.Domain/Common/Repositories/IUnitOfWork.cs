using System.Data;

namespace LeagueOfStats.Domain.Common.Repositories;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    IDbTransaction BeginTransaction();
}