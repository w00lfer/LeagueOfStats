using LeagueOfStats.Domain.Common.Repositories;

namespace LeagueOfStats.Domain.Discounts;

public interface IDiscountRepository
{
    Task<Discount?> GetByIdAsync(Guid id);
    
    Task<IEnumerable<Discount>> GetAllAsync();

    Task AddAsync(Discount discount);
}