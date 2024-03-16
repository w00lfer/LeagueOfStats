using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMasteries;

public class GetSummonerChampionMasteriesQueryValidator : AbstractValidator<GetSummonerChampionMasteriesQuery>
{
    public GetSummonerChampionMasteriesQueryValidator()
    {
        RuleFor(x => x.SummonerId)
            .NotEmpty();
    }
}