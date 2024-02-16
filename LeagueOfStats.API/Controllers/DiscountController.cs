using LeagueOfStats.API.Extensions;
using LeagueOfStats.Application.Discounts.Queries.GetDiscounts;
using LeagueOfStats.Domain.Discounts.Enums;
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
    [ApiExplorerSettings(IgnoreApi = true)]
    public Task<IActionResult> GetDiscountById(
        Guid id,
        DiscountType? discountType = null) =>
        throw new NotImplementedException();
}