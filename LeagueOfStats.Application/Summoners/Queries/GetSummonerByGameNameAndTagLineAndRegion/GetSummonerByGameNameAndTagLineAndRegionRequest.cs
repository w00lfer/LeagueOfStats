using LanguageExt;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Errors;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerByGameNameAndTagLineAndRegion;

public record GetSummonerByGameNameAndTagLineAndRegionRequest(
    string GameName,
    string TagLine,
    Region Region)
: IRequest<Either<Error, SummonerDto>>;

public class GetSummonerByGameNameAndTagLineAndRegionRequestHandler : IRequestHandler<GetSummonerByGameNameAndTagLineAndRegionRequest, Either<Error, SummonerDto>>
{
    private readonly ISummonerApplicationService _summonerApplicationService;

    public GetSummonerByGameNameAndTagLineAndRegionRequestHandler(ISummonerApplicationService summonerApplicationService)
    {
        _summonerApplicationService = summonerApplicationService;
    }

    public Task<Either<Error, SummonerDto>> Handle(GetSummonerByGameNameAndTagLineAndRegionRequest request, CancellationToken cancellationToken) => 
        _summonerApplicationService.GetSummonerByGameNameAndTagLineAndRegion(request.GameName, request.TagLine, request.Region)
            .BindAsync(summoner => Either<Error, SummonerDto>.Right(MapToSummonerDto(summoner)));

    private SummonerDto MapToSummonerDto(Summoner summoner) => 
        new(
            summoner.Id,
            summoner.AccountId,
            summoner.SummonerId,
            summoner.Name,
            summoner.ProfileIconId,
            summoner.Puuid,
            summoner.SummonerLevel,
            summoner.SummonerName,
            summoner.LastUpdated);
}