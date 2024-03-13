using LeagueOfStats.Domain.Discounts;
using LeagueOfStats.Infrastructure.ApplicationDbContexts;
using Microsoft.EntityFrameworkCore;

namespace LeagueOfStats.Infrastructure.Discounts;

public class DiscountRepository : IDiscountRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public DiscountRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public Task<Discount?> GetByIdAsync(Guid id) =>
        _applicationDbContext
            .Discounts
            .Include(d => d.DiscountedChampions)
            .Include(d => d.DiscountedSkins)
            .SingleOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<Discount>> GetAllAsync() =>
        await _applicationDbContext
            .Discounts
            .Include(d => d.DiscountedChampions)
            .Include(d => d.DiscountedSkins)
            .ToListAsync();
    
    public async Task AddAsync(Discount discount)
    {
        await _applicationDbContext.Discounts.AddAsync(discount);

        await _applicationDbContext.SaveChangesAsync();
    }
}