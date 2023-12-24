using FluentValidation;
using LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMasteries;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerChampionMastery;

public class GetSummonerChampionMasteryQueryValidator : AbstractValidator<GetSummonerChampionMasteriesQuery>
{
    public GetSummonerChampionMasteryQueryValidator()
    {
        RuleFor(x => x.SummonerId)
            .NotEmpty();
    }
}