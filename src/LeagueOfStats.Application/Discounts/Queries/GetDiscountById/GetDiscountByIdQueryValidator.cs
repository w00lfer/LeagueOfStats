using FluentValidation;

namespace LeagueOfStats.Application.Discounts.Queries.GetDiscountById;

public class GetDiscountByIdQueryValidator : AbstractValidator<GetDiscountByIdQuery>
{
    public GetDiscountByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}