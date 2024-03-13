using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Discounts;
using MediatR;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscountById;

public record GetDiscountByIdQuery(
    Guid Id)
    : IRequest<Result<Discount>>;

public class GetDiscountByIdQueryHandler 
    : IRequestHandler<GetDiscountByIdQuery, Result<Discount>>
{
    private readonly IDiscountDomainService _discountDomainService;

    public Task<Result<Discount>> Handle(
        GetDiscountByIdQuery request,
        CancellationToken cancellationToken) =>
        _discountDomainService.GetByIdAsync(request.Id);
}