using LeagueOfStats.Application.Discounts.RiotGamesShopClient;
using LeagueOfStats.Domain.Common.Rails.Results;
using MediatR;
using NodaTime;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscounts;

public record GetDiscountsQuery()
    : IRequest<Result<IEnumerable<RiotGamesShopDiscount>>>;

public class GetDiscountsQueryHandler 
    : IRequestHandler<GetDiscountsQuery, Result<IEnumerable<RiotGamesShopDiscount>>>
{
    private readonly IRiotGamesShopClient _riotGamesShopClient;

    public GetDiscountsQueryHandler(IRiotGamesShopClient riotGamesShopClient)
    {
        _riotGamesShopClient = riotGamesShopClient;
    }

    public Task<Result<IEnumerable<RiotGamesShopDiscount>>> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
    { 
        return _riotGamesShopClient.GetCurrentDiscountsAsync()
        .Bind(discounts =>
        {
            return Result.Success(discounts);
        });
    }
}