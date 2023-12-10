using LanguageExt;
using LeagueOfStats.Domain.Common.Errors;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerById;

public record GetSummonerByIdRequest(
        Guid Id)
    : IRequest<Either<Error, SummonerDto>>;

public class GetSummonerByIdRequestRequestHandler : IRequestHandler<GetSummonerByIdRequest, Either<Error, SummonerDto>>
{
    private readonly ISummonerApplicationService _summonerApplicationService;

    public GetSummonerByIdRequestRequestHandler(ISummonerApplicationService summonerApplicationService)
    {
        _summonerApplicationService = summonerApplicationService;
    }

    public Task<Either<Error, SummonerDto>> Handle(GetSummonerByIdRequest request, CancellationToken cancellationToken) => 
        _summonerApplicationService.GetSummonerById(request.Id)
            .BindAsync(summoner => Either<Error, SummonerDto>.Right(MapToSummonerDto(summoner)));

    private SummonerDto MapToSummonerDto(Summoner summoner) => 
        new(
            summoner.Id.Value,
            summoner.AccountId,
            summoner.SummonerId,
            summoner.Name,
            summoner.ProfileIconId,
            summoner.Puuid,
            summoner.SummonerLevel,
            summoner.SummonerName,
            summoner.LastUpdated);
}