using FluentValidation;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMasteries;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMastery;

public class GetSummonerChampionMasteriesQueryValidator : AbstractValidator<GetSummonerChampionMasteriesQuery>
{
    public GetSummonerChampionMasteriesQueryValidator()
    {
        RuleFor(x => x.SummonerId)
            .NotEmpty();
    }
}