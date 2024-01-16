using LeagueOfStats.Domain.Discounts.Enums;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace LeagueOfStats.API.Controllers;

[ApiController]
[Route("Discounts")]
[ApiExplorerSettings(IgnoreApi = true)]
public class DiscountController : ControllerBase
{
    [HttpGet("")]
    public Task<IActionResult> GetDiscounts(
        [FromQuery] LocalDate? discountsFromDate = null) =>
        throw new NotImplementedException();
    
    [HttpGet("{id}")]
    public Task<IActionResult> GetDiscountById(
        Guid id,
        DiscountType? discountType = null) =>
        throw new NotImplementedException();
}