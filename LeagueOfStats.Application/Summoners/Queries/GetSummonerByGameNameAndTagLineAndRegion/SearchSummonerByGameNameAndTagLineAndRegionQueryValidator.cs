using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerByGameNameAndTagLineAndRegion;

public class SearchSummonerByGameNameAndTagLineAndRegionQueryValidator : AbstractValidator<SearchSummonerByGameNameAndTagLineAndRegionQuery>
{
    public SearchSummonerByGameNameAndTagLineAndRegionQueryValidator()
    {
        RuleFor(x => x.GameName)
            .NotEmpty();
        
        RuleFor(x => x.TagLine)
            .NotEmpty();
    }
}