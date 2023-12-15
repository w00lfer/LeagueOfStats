using LanguageExt;
using LeagueOfStats.Domain.Common.Errors;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Commands.RefreshSummonerCommand;

public record RefreshSummonerCommand
    (Guid Id)
: IRequest<Option<Error>>;

public class RefreshSummonerCommandHandler : IRequestHandler<RefreshSummonerCommand, Option<Error>>
{
    private readonly ISummonerApplicationService _summonerApplicationService;

    public RefreshSummonerCommandHandler(ISummonerApplicationService summonerApplicationService)
    {
        _summonerApplicationService = summonerApplicationService;
    }

    public Task<Option<Error>> Handle(RefreshSummonerCommand request, CancellationToken cancellationToken) =>
        _summonerApplicationService.RefreshSummonerDataBySummonerId(request.Id);
}