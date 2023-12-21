using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMastery;

public class GetSummonerChampionMasteryQueryValidator : AbstractValidator<GetSummonerChampionMasteryQuery>
{
    public GetSummonerChampionMasteryQueryValidator()
    {
        RuleFor(x => x.SummonerId)
            .NotEmpty();
    }
}