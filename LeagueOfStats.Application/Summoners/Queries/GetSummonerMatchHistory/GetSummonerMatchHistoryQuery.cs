using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Summoners;
using MediatR;
using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public record GetSummonerMatchHistoryQuery(
    Guid SummonerId,
    Instant GameEndedAt,
    GameType GameType,
    int Limit)
    : IRequest<Result<IEnumerable<MatchHistoryDto>>>;

public class GetSummonerMatchHistoryQueryHandler : IRequestHandler<GetSummonerMatchHistoryQuery, Result<IEnumerable<MatchHistoryDto>>>
{
    private readonly IValidator<GetSummonerMatchHistoryQuery> _getSummonerMatchHistoryQueryValidator;
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IRiotClient _riotClient;
    private readonly IMatchDomainService _matchDomainService;

    public GetSummonerMatchHistoryQueryHandler(
        IValidator<GetSummonerMatchHistoryQuery> getSummonerMatchHistoryQueryValidator,
        ISummonerDomainService summonerDomainService,
        IRiotClient riotClient,
        IMatchDomainService matchDomainService)
    {
        _getSummonerMatchHistoryQueryValidator = getSummonerMatchHistoryQueryValidator;
        _summonerDomainService = summonerDomainService;
        _riotClient = riotClient;
        _matchDomainService = matchDomainService;
    }

    public Task<Result<IEnumerable<MatchHistoryDto>>> Handle(GetSummonerMatchHistoryQuery query,
        CancellationToken cancellationToken) =>
        _getSummonerMatchHistoryQueryValidator.ValidateAsync(query)
            .Bind(() => _summonerDomainService.GetByIdAsync(query.SummonerId))
            .Bind(summoner => _riotClient.GetSummonerMatchHistory(
                new GetSummonerMatchHistoryDto(
                    summoner.Region,
                    summoner.Puuid,
                    query.Limit,
                    query.GameEndedAt,
                    query.GameType)))
            .Bind(async matchesFromRiotApi =>
            {
                var matchesToAdd = matchesFromRiotApi
                    .Select(m => new AddMatchDto(m.Metadata.MatchId,
                        Instant.FromUnixTimeMilliseconds(m.Info.GameEndTimestamp!.Value)));

                var addedMatches = await _matchDomainService.AddMatches(matchesToAdd);

                return Result.Success(addedMatches);
            })
            .Map(MapToMatchHistoryDtos);

    private IEnumerable<MatchHistoryDto> MapToMatchHistoryDtos(IEnumerable<Match> matches) =>
        matches.Select(m => new MatchHistoryDto(m.RiotMatchId, m.GameEndTimestamp));
} 