using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Domain.Discounts;

public interface IDiscountDomainService
{
    public Task<Result<Discount>> GetByIdAsync(Guid id);
    
    public Task<IEnumerable<Discount>> GetAllAsync();
    
    public Task<Discount> AddAsync(AddDiscountDto addDiscountDto);
}