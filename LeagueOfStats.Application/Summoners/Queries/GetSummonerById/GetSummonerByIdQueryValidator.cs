using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerById;

public class GetSummonerByIdQueryValidator : AbstractValidator<GetSummonerByIdQuery>
{
    public GetSummonerByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}