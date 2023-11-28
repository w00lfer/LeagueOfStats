using LanguageExt;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.Enums;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Errors;
using MediatR;

namespace LeagueOfStats.Application.SummonerChampionMastery.Queries.GetSummonersByName;

public record GetSummonerRequest(
        string GameName,
        string TagLine,
        string Region)
    : IRequest<Either<Error, SummonerDto>>;

public class GetSummonerRequestHandler : IRequestHandler<GetSummonerRequest, Either<Error, SummonerDto>>
{
    private readonly IRiotClient _riotClient;

    public GetSummonerRequestHandler(IRiotClient riotClient)
    {
        _riotClient = riotClient;
    }

    public Task<Either<Error, SummonerDto>> Handle(GetSummonerRequest getSummonerRequest, CancellationToken cancellationToken) =>
        GetRegion(getSummonerRequest.Region)
            .BindAsync(region => _riotClient.GetSummonerByGameNameAndTaglineAsync(getSummonerRequest.GameName, getSummonerRequest.TagLine, region))
            .BindAsync(summoner => Either<Error, SummonerDto>.Right(new SummonerDto(summoner.AccountId, summoner.Id, summoner.Name, summoner.ProfileIconId, summoner.Puuid, summoner.RevisionDate, summoner.SummonerLevel, $"{getSummonerRequest.GameName}#{getSummonerRequest.TagLine}")));

    private Either<Error, Region> GetRegion(string regionString) =>
        Enum.TryParse<Region>(regionString, out var region)
            ? region
            : new ApplicationError("Region does not exist.");
}