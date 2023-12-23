using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerById;

public record GetSummonerByIdQuery(
    Guid Id)
: IRequest<Result<SummonerDto>>;

public class GetSummonerByIdRequestQueryHandler : IRequestHandler<GetSummonerByIdQuery, Result<SummonerDto>>
{
    private readonly IValidator<GetSummonerByIdQuery> _getSummonerByIdQueryValidator;
    private readonly ISummonerDomainService _summonerDomainService;

    public GetSummonerByIdRequestQueryHandler(
        IValidator<GetSummonerByIdQuery> getSummonerByIdQueryValidator,
        ISummonerDomainService summonerDomainService)
    {
        _getSummonerByIdQueryValidator = getSummonerByIdQueryValidator;
        _summonerDomainService = summonerDomainService;
    }

    public Task<Result<SummonerDto>> Handle(GetSummonerByIdQuery query, CancellationToken cancellationToken) =>
        _getSummonerByIdQueryValidator.ValidateAsyncTwo(query)
            .Bind(() => _summonerDomainService.GetByIdAsyncTwo(query.Id))
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
            summoner.LastUpdated);
}