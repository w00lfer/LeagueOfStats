using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchById;

public class GetSummonerMatchByIdQueryValidator : AbstractValidator<GetSummonerMatchByIdQuery>
{
    public GetSummonerMatchByIdQueryValidator()
    {
        RuleFor(x => x.SummonerId)
            .NotEmpty();
        
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}