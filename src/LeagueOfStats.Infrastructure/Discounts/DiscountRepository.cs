using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using LeagueOfStats.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace LeagueOfStats.Infrastructure.Discounts;

public class DiscountRepository : AsyncRepositoryBase<Discount>, IDiscountRepository
{
    public DiscountRepository(ApplicationDbContext applicationDbContext)
        : base(applicationDbContext)
    {
    }

    public Task<bool> DoesDiscountInGivenLocalDateTimeExistAsync(LocalDateTime localDateTime) =>
        _applicationDbContext.Discounts
            .AnyAsync(d => localDateTime >= d.StartDateTime && localDateTime <= d.EndDateTime);
}