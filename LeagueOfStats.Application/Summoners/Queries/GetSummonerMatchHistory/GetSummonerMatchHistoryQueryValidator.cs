using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public class GetSummonerMatchHistoryQueryValidator : AbstractValidator<GetSummonerMatchHistoryQuery>
{
    public GetSummonerMatchHistoryQueryValidator()
    {
        RuleFor(x => x.SummonerId)
            .NotEmpty();
    }
}