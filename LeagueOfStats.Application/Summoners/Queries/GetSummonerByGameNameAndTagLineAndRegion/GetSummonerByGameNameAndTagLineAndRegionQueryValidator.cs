using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerByGameNameAndTagLineAndRegion;

public class GetSummonerByGameNameAndTagLineAndRegionQueryValidator : AbstractValidator<GetSummonerByGameNameAndTagLineAndRegionQuery>
{
    public GetSummonerByGameNameAndTagLineAndRegionQueryValidator()
    {
        RuleFor(x => x.GameName)
            .NotEmpty();
        
        RuleFor(x => x.TagLine)
            .NotEmpty();
    }
}