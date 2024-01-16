using LeagueOfStats.Application.Discounts.RiotGamesShopClient;
using LeagueOfStats.Domain.Common.Rails.Results;
using MediatR;
using NodaTime;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscounts;

public record GetDiscountsQuery(
    LocalDate DiscountsFromDate)
    : IRequest<Result<IEnumerable<DiscountDto>>>;

public class GetDiscountsQueryHandler 
    : IRequestHandler<GetDiscountsQuery, Result<IEnumerable<DiscountDto>>>
{
    private readonly IRiotGamesShopClient _riotGamesShopClient;

    public GetDiscountsQueryHandler(IRiotGamesShopClient riotGamesShopClient)
    {
        _riotGamesShopClient = riotGamesShopClient;
    }

    public Task<Result<IEnumerable<DiscountDto>>> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
    {
        return _riotGamesShopClient.GetCurrentDiscountsAsync()
            .Bind(discounts =>
            {
                
            });
    }
}