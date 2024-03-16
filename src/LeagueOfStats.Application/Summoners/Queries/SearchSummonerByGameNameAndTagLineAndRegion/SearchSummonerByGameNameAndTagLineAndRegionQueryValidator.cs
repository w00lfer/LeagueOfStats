using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Queries.SearchSummonerByGameNameAndTagLineAndRegion;

public class SearchSummonerByGameNameAndTagLineAndRegionQueryValidator
    : AbstractValidator<SearchSummonerByGameNameAndTagLineAndRegionQuery>
{
    public SearchSummonerByGameNameAndTagLineAndRegionQueryValidator()
    {
        RuleFor(x => x.GameName)
            .NotEmpty();
        
        RuleFor(x => x.TagLine)
            .NotEmpty();
    }
}