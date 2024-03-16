using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerLiveGame;

public class GetSummonerLiveGameQueryValidator : AbstractValidator<GetSummonerLiveGameQuery>
{
    public GetSummonerLiveGameQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}