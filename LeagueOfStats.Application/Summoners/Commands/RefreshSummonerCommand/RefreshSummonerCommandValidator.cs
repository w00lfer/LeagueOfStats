using FluentValidation;

namespace LeagueOfStats.Application.Summoners.Commands.RefreshSummonerCommand;

public class RefreshSummonerCommandValidator : AbstractValidator<RefreshSummonerCommand>
{
    public RefreshSummonerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}