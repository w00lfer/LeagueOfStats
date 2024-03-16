using LeagueOfStats.Domain.Common.Repositories;
using NodaTime;

namespace LeagueOfStats.Domain.Discounts;

public interface IDiscountRepository : IAsyncRepository<Discount>
{
    Task<bool> DoesDiscountInGivenLocalDateTimeExistAsync(LocalDateTime localDateTime);
}