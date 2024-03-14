using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Discounts.Queries.GetDiscountById;
using LeagueOfStats.Application.Discounts.Queries.GetDiscounts;
using LeagueOfStats.Application.RiotGamesShopClient.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Discounts")]
public class DiscountController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IClock _clock;

    public DiscountController(IMediator mediator, IClock clock)
    {
        _mediator = mediator;
        _clock = clock;
    }

    [HttpGet("")]
    public Task<IActionResult> GetDiscounts() =>
        _mediator.Send(new GetDiscountsQuery())
            .ToIActionResult(this);

    [HttpGet("{id}")]
    public Task<IActionResult> GetDiscountById(Guid id, DiscountType? discountType = null) =>
        _mediator.Send(new GetDiscountByIdQuery(id))
            .ToIActionResult(this);
}