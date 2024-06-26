using LeagueOfStats.Application.Common;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using MediatR;
using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerById;

public record GetSummonerByIdQuery(
    Guid Id)
    : IRequest<Result<SummonerDto>>;

public class GetSummonerByIdRequestQueryHandler
    : IRequestHandler<GetSummonerByIdQuery, Result<SummonerDto>>
{
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IEntityUpdateLockoutService _entityUpdateLockoutService;

    public GetSummonerByIdRequestQueryHandler(
        ISummonerDomainService summonerDomainService,
        IEntityUpdateLockoutService entityUpdateLockoutService)
    {
        _summonerDomainService = summonerDomainService;
        _entityUpdateLockoutService = entityUpdateLockoutService;
    }

    public Task<Result<SummonerDto>> Handle(
        GetSummonerByIdQuery query,
        CancellationToken cancellationToken) => 
        _summonerDomainService.GetByIdAsync(query.Id)
            .Map(MapToSummonerDto);

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
            summoner.LastUpdated,
            summoner.LastUpdated.Plus(Duration.FromMinutes(
                _entityUpdateLockoutService.GetSummonerUpdateLockoutInMinutes())));
}