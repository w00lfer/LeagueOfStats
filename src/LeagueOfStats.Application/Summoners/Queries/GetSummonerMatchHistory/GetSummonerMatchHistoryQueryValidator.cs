using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public class GetSummonerMatchHistoryQueryValidator : AbstractValidator<GetSummonerMatchHistorySummaryQuery>
{
    public GetSummonerMatchHistoryQueryValidator()
    {
        RuleFor(x => x.SummonerId)
            .NotEmpty();
        
        RuleFor(x => x.GameEndedAt)
            .NotEmpty();
        
        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 20);
    }
}