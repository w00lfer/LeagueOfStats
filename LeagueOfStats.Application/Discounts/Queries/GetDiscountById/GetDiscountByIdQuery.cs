using LeagueOfStats.Domain.Common.Rails.Results;
using MediatR;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscountById;

public record GetDiscountByIdQuery(
    Guid Id)
    : IRequest<Result<object>>;

public class GetDiscountByIdQueryHandler 
    : IRequestHandler<GetDiscountByIdQuery, Result<object>>
{
    public Task<Result<object>> Handle(
        GetDiscountByIdQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}