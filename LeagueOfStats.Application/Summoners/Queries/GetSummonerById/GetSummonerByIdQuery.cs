using LanguageExt;
using LeagueOfStats.Domain.Common.Errors;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerById;

public record GetSummonerByIdQuery(
    Guid Id)
: IRequest<Either<Error, SummonerDto>>;

public class GetSummonerByIdRequestQueryHandler : IRequestHandler<GetSummonerByIdQuery, Either<Error, SummonerDto>>
{
    private readonly ISummonerDomainService _summonerDomainService;

    public GetSummonerByIdRequestQueryHandler(ISummonerDomainService summonerDomainService)
    {
        _summonerDomainService = summonerDomainService;
    }

    public Task<Either<Error, SummonerDto>> Handle(GetSummonerByIdQuery query, CancellationToken cancellationToken) => 
        _summonerDomainService.GetByIdAsync(query.Id)
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