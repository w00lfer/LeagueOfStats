using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Discounts;
using MediatR;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscounts;

public record GetDiscountsQuery()
    : IRequest<Result<IEnumerable<DiscountSummaryDto>>>;

public class GetDiscountsQueryHandler 
    : IRequestHandler<GetDiscountsQuery, Result<IEnumerable<DiscountSummaryDto>>>
{
    private readonly IDiscountDomainService _discountDomainService;
    
    public GetDiscountsQueryHandler(IDiscountDomainService discountDomainService)
    {
        _discountDomainService = discountDomainService;
    }

    public async Task<Result<IEnumerable<DiscountSummaryDto>>> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
    {
        List<Discount> discounts = (await _discountDomainService.GetAllAsync()).ToList();

        return Result.Success(MapDiscountsToDiscountSummaries(discounts));
    }

    private static IEnumerable<DiscountSummaryDto> MapDiscountsToDiscountSummaries(IEnumerable<Discount> discounts) =>
        discounts.Select(d => new DiscountSummaryDto(
            d.Id,
            d.StartDateTime,
            d.EndDateTime));
}