using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchById;

public record GetSummonerMatchByIdQuery(
    Guid SummonerId,
    Guid Id)
    : IRequest<Result<SummonerMatchDto>>;

public class GetSummonerMatchByIdQueryHandler : IRequestHandler<GetSummonerMatchByIdQuery, Result<SummonerMatchDto>>
{
    private readonly IValidator<GetSummonerMatchByIdQuery> _getSummonerMatchByIdQuery;
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IRiotClient _riotClient;
    private readonly IMatchDomainService _matchDomainService;

    public GetSummonerMatchByIdQueryHandler(
        IValidator<GetSummonerMatchByIdQuery> getSummonerMatchByIdQuery,
        ISummonerDomainService summonerDomainService,
        IRiotClient riotClient,
        IMatchDomainService matchDomainService)
    {
        _getSummonerMatchByIdQuery = getSummonerMatchByIdQuery;
        _summonerDomainService = summonerDomainService;
        _riotClient = riotClient;
        _matchDomainService = matchDomainService;
    }

    public Task<Result<SummonerMatchDto>> Handle(GetSummonerMatchByIdQuery query, CancellationToken cancellationToken) =>
        _getSummonerMatchByIdQuery.ValidateAsync(query)
            .Bind(() => _summonerDomainService.GetByIdAsync(query.SummonerId))
            .Bind(_ => _matchDomainService.GetByIdAsync(query.Id))
            .Map(MapToSummonerMatchDto);
    
    private SummonerMatchDto MapToSummonerMatchDto(Match match) =>
        new (match.Id, match.RiotMatchId, match.SummonerIds ,match.GameEndTimestamp);
} 