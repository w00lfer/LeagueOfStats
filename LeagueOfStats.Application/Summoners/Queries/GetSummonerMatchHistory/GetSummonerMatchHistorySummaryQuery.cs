using LeagueOfStats.Application.Common.Enums;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using LeagueOfStats.Domain.Summoners;
using MediatR;
using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public record GetSummonerMatchHistorySummaryQuery(
    Guid SummonerId,
    Instant GameEndedAt,
    QueueFilter QueueFilter,
    int Limit)
    : IRequest<Result<IEnumerable<SummonerMatchHistorySummaryDto>>>;

public class GetSummonerMatchHistorySummaryQueryHandler : IRequestHandler<GetSummonerMatchHistorySummaryQuery, Result<IEnumerable<SummonerMatchHistorySummaryDto>>>
{
    private readonly IValidator<GetSummonerMatchHistorySummaryQuery> _getSummonerMatchHistoryQueryValidator;
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IRiotClient _riotClient;
    private readonly IMatchDomainService _matchDomainService;

    public GetSummonerMatchHistorySummaryQueryHandler(
        IValidator<GetSummonerMatchHistorySummaryQuery> getSummonerMatchHistoryQueryValidator,
        ISummonerDomainService summonerDomainService,
        IRiotClient riotClient,
        IMatchDomainService matchDomainService)
    {
        _getSummonerMatchHistoryQueryValidator = getSummonerMatchHistoryQueryValidator;
        _summonerDomainService = summonerDomainService;
        _riotClient = riotClient;
        _matchDomainService = matchDomainService;
    }

    public Task<Result<IEnumerable<SummonerMatchHistorySummaryDto>>> Handle(GetSummonerMatchHistorySummaryQuery query, CancellationToken cancellationToken) =>
        _getSummonerMatchHistoryQueryValidator.ValidateAsync(query)
            .Bind(() => _summonerDomainService.GetByIdAsync(query.SummonerId))
            .Bind(summoner => _riotClient.GetSummonerMatchHistorySummary(new GetSummonerMatchHistoryDto(
                    summoner.Region,
                    summoner.Puuid,
                    query.Limit,
                    query.GameEndedAt,
                    query.QueueFilter))
                .Bind(matchesFromRiotApi => GetOrCreateSummonersForMatchesByPuuid(matchesFromRiotApi, summoner.Region)
                    .Bind(summoners =>
                    {
                        var addMatchDtos = matchesFromRiotApi.Select(m =>
                        {
                            var summonersPuuidsInMatch = m.Info.Participants.Select(p => p.Puuid);
                            
                            return new AddMatchDto(
                                m.Metadata.MatchId,
                                summoners.Where(s => summonersPuuidsInMatch.Contains(s.Puuid)),
                                Instant.FromUnixTimeMilliseconds(m.Info.GameEndTimestamp!.Value));
                        });

                        return _matchDomainService.AddMatchesAsync(addMatchDtos);
                    })))
            .Map(MapToMatchHistorySummaryDtos);

    private async Task<Result<List<Summoner>>> GetOrCreateSummonersForMatchesByPuuid(IEnumerable<Camille.RiotGames.MatchV5.Match> matchesFromRiotApi, Region region)
    {
        var uniqueSummoners = matchesFromRiotApi.SelectMany(m => m.Info.Participants).Select(p => new SummonerInfoDto(p.Puuid, p.RiotIdName, p.RiotIdTagline)).Distinct().ToList();

        var getSummonerByPuuidResults = await Task.WhenAll(uniqueSummoners.Select(s => _summonerDomainService.GetByPuuidAsync(s.Puuid)));

        var existingSummoners = getSummonerByPuuidResults.Where(r => r.IsSuccess).Select(r => r.Value).ToList();
        var existingSummonersPuuids = existingSummoners.Select(s => s.Puuid).ToList();

        Result<Summoner>[] createSummonerResults = await Task.WhenAll(uniqueSummoners.Where(s => existingSummonersPuuids.Contains(s.Puuid) is false).Select(s => CreateSummonerUsingDataFromRiotApiAsync(s, region)));

        if (createSummonerResults.Any(r => r.IsFailure))
        {
            return new ApplicationError(string.Join(
                Environment.NewLine,
                createSummonerResults.Where(r => r.IsFailure).Select(r => r.AggregatedErrorMessages)));
        }

        return Result.Success(createSummonerResults.Select(r => r.Value).Concat(existingSummoners).ToList());
    }

    private Task<Result<Summoner>> CreateSummonerUsingDataFromRiotApiAsync(
        SummonerInfoDto summonerInfoDto,
        Region region) =>
        _riotClient.GetSummonerByPuuidAsync(summonerInfoDto.Puuid, region)
            .Bind(summonerFromRiotApi => _riotClient.GetSummonerChampionMasteryByPuuid(summonerFromRiotApi.Puuid, region)
                .Bind(async summonerChampionMasteriesFromRiotApi =>
                {
                    var createSummonerDto = new CreateSummonerDto(
                        summonerFromRiotApi.Id,
                        summonerFromRiotApi.AccountId,
                        summonerFromRiotApi.Name,
                        summonerFromRiotApi.ProfileIconId,
                        summonerFromRiotApi.Puuid,
                        summonerFromRiotApi.SummonerLevel,
                        summonerInfoDto.GameName,
                        summonerInfoDto.TagLine,
                        region,
                        summonerChampionMasteriesFromRiotApi.Select(c =>
                            new UpdateChampionMasteryDto(
                                (int)c.ChampionId,
                                c.ChampionLevel,
                                c.ChampionPoints,
                                c.ChampionPointsSinceLastLevel,
                                c.ChampionPointsUntilNextLevel,
                                c.ChestGranted,
                                c.LastPlayTime,
                                c.TokensEarned)));

                    var summoner = await _summonerDomainService.CreateAsync(createSummonerDto);

                    return Result.Success(summoner);
                }));


    private AddMatchDto MapMatchFromRiotApiToAddMatchdto(Camille.RiotGames.MatchV5.Match matchFromRiotApi, List<Summoner> participatedSummonersInMatch)
    {
        var summonersPuuidsInMatch = m.Info.Participants.Select(p => p.Puuid);

        List<AddParticipantDto> addParticipantDtos = new List<AddParticipantDto>();

        var addTeamDtos = matchFromRiotApi.Info.Teams.Select(team =>
        {
            return new AddTeamDto(
                new AddObjectivesDto(
                    new AddObjectiveDto(team.Objectives.Baron.First, team.Objectives.Baron.Kills),
                    new AddObjectiveDto(team.Objectives.Champion.First, team.Objectives.Champion.Kills),
                    new AddObjectiveDto(team.Objectives.Dragon.First, team.Objectives.Dragon.Kills),
                    team.Objectives.Horde is null
                        ? null
                        : new AddObjectiveDto(team.Objectives.Horde.First, team.Objectives.Horde.Kills),
                    new AddObjectiveDto(team.Objectives.Inhibitor.First, team.Objectives.Inhibitor.Kills),
                    new AddObjectiveDto(team.Objectives.RiftHerald.First, team.Objectives.RiftHerald.Kills),
                    new AddObjectiveDto(team.Objectives.Tower.First, team.Objectives.Tower.Kills)),
                new List<AddBanDto>(),
                team.TeamId.ToSide(),
                team.Win);
        });

        var addMatchDto = new AddMatchDto(
            matchFromRiotApi.Metadata.MatchId,
            matchFromRiotApi.Info.GameVersion,
            Duration.FromSeconds(matchFromRiotApi.Info.GameDuration),
            Instant.FromUnixTimeMilliseconds(matchFromRiotApi.Info.GameStartTimestamp),
            Instant.FromUnixTimeMilliseconds(matchFromRiotApi.Info.GameEndTimestamp!.Value),
            matchFromRiotApi.Info.GameMode.ToGameMode(), // Map to Game Mode
            matchFromRiotApi.Info.GameType.ToGameType(), // Map to game Type
            matchFromRiotApi.Info.MapId.ToMap(), // Map to Map
            matchFromRiotApi.Info.PlatformId,
            matchFromRiotApi.Info.QueueId.ToQueue(), //Map to queue
            matchFromRiotApi.Info.TournamentCode,
            addParticipantDtos,
            addTeamDtos);
        
        return new AddMatchDto(
            
            m.Metadata.MatchId,
            summoners.Where(s => summonersPuuidsInMatch.Contains(s.Puuid)),
            Instant.FromUnixTimeMilliseconds(m.Info.GameEndTimestamp!.Value));
    }
    
    private IEnumerable<SummonerMatchHistorySummaryDto> MapToMatchHistorySummaryDtos(IEnumerable<Match> matches) =>
        matches.Select(m => new SummonerMatchHistorySummaryDto(m.Id, m.RiotMatchId, m.SummonerIds, m.GameEndTimestamp));

    private record SummonerInfoDto(
        string Puuid,
        string GameName,
        string TagLine);
} 