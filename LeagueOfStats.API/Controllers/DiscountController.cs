using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Discounts.Queries.GetDiscountById;
using LeagueOfStats.Application.Discounts.Queries.GetDiscounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Discounts")]
public class DiscountController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiscountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    public Task<IActionResult> GetDiscounts() =>
        _mediator.Send(new GetDiscountsQuery())
            .ToIActionResult(this);

    [HttpGet("{id}")]
    public Task<IActionResult> GetDiscountById(Guid id) =>
        _mediator.Send(new GetDiscountByIdQuery(id))
            .ToIActionResult(this);
}