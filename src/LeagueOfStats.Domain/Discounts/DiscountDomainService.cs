using LeagueOfStats.Domain.Common.Rails.Errors;
using LeagueOfStats.Domain.Common.Rails.Results;

namespace LeagueOfStats.Domain.Discounts;

public class DiscountDomainService : IDiscountDomainService
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountDomainService(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<Result<Discount>> GetByIdAsync(Guid id)
    {
        Discount? discount = await _discountRepository.GetByIdAsync(id);
        
        return discount is not null
            ? discount
            : new EntityNotFoundError($"Discount with Id={id} does not exist.");
    }

    public Task<IEnumerable<Discount>> GetAllAsync() => 
        _discountRepository.GetAllAsync();

    public async Task<Discount> AddAsync(AddDiscountDto addDiscountDto)
    {
        var discount = new Discount(addDiscountDto);

        await _discountRepository.AddAsync(discount);

        return discount;
    }
}