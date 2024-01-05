using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Matches.Enums;
using LeagueOfStats.Domain.Matches.Participants;
using LeagueOfStats.Domain.Summoners;
using MediatR;
using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchById;

public record GetSummonerMatchByIdQuery(
    Guid SummonerId,
    Guid Id)
    : IRequest<Result<MatchDetailsDto>>;

public class GetSummonerMatchByIdQueryHandler
    : IRequestHandler<GetSummonerMatchByIdQuery, Result<MatchDetailsDto>>
{
    private readonly IValidator<GetSummonerMatchByIdQuery> _getSummonerMatchByIdQuery;
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly ISummonerRepository _summonerRepository;
    private readonly IMatchDomainService _matchDomainService;
    private readonly IChampionRepository _championRepository;

    public GetSummonerMatchByIdQueryHandler(
        IValidator<GetSummonerMatchByIdQuery> getSummonerMatchByIdQuery,
        ISummonerDomainService summonerDomainService,
        ISummonerRepository summonerRepository,
        IMatchDomainService matchDomainService,
        IChampionRepository championRepository)
    {
        _getSummonerMatchByIdQuery = getSummonerMatchByIdQuery;
        _summonerDomainService = summonerDomainService;
        _summonerRepository = summonerRepository;
        _matchDomainService = matchDomainService;
        _championRepository = championRepository;
    }

    public Task<Result<MatchDetailsDto>> Handle(
        GetSummonerMatchByIdQuery query,
        CancellationToken cancellationToken) =>
        _getSummonerMatchByIdQuery.ValidateAsync(query)
            .Bind(() => _summonerDomainService.GetByIdAsync(query.SummonerId))
            .Bind(_ => _matchDomainService.GetByIdAsync(query.Id))
            .Map(match => MapToMatchDetailsDtoAsync(match, query.SummonerId));
    
    private async Task<MatchDetailsDto> MapToMatchDetailsDtoAsync(
        Match match,
        Guid summonerId) => 
        new(
            match.Id,
            await MapParticipantsToMatchDetailsTeamDtosAsync(
                match.Participants,
                match.GameMode,
                match.GameDuration,
                summonerId),
            match.GameVersion,
            match.GameDuration,
            match.GameStartTimeStamp,
            match.GameEndTimestamp,
            match.GameMode,
            match.GameType,
            match.Map);

    private async Task<IEnumerable<MatchDetailsTeamDto>> MapParticipantsToMatchDetailsTeamDtosAsync(
        IEnumerable<Participant> participants,
        GameMode gameMode,
        Duration gameDuration,
        Guid summonerId) 
    {
        var champions = (await _championRepository.GetAllAsync()).ToList();
        var summoners = (await _summonerRepository
                .GetAllAsync(participants.Select(p => p.SummonerId)
                .ToArray()))
            .ToList();
        
        if (gameMode is GameMode.Arena)
        {
            return participants.GroupBy(p => p.PlayerSubteamId).Select(g =>
                new MatchDetailsTeamDto(
                    g.Select(p =>
                    {
                        Champion champion = champions.Single(c => c.Id == p.ChampionId);
                        Summoner summoner = summoners.Single(s => s.Id == p.SummonerId);
                        return new MatchDetailsTeamParticipantDto(
                            champion.Id,
                            summoner.Id,
                            summoner.SummonerName.ToString(),
                            champion.Name,
                            champion.ChampionImage.FullFileName,
                            p.ChampLevel,
                            p.Kills,
                            p.Deaths,
                            p.Assists,
                            (p.Kills + p.Assists) / p.Deaths,
                            p.TotalMinionsKilled,
                            p.TotalMinionsKilled / gameDuration.Minutes,
                            p.Item0,
                            p.Item1,
                            p.Item2,
                            p.Item3,
                            p.Item4,
                            p.Item5,
                            p.Item6,
                            p.SummonerId == summonerId);
                    }),
                    g.Select(p => p.Side).Distinct().Single(),
                    g.Select(p => p.Win).Distinct().Single()));
        }

        return participants.GroupBy(p => p.Side).Select(g =>
            new MatchDetailsTeamDto(
                g.Select(p =>
                {
                    Champion champion = champions.Single(c => c.Id == p.ChampionId);
                    Summoner summoner = summoners.Single(s => s.Id == p.SummonerId);
                    return new MatchDetailsTeamParticipantDto(
                        champion.Id,
                        summoner.Id,
                        summoner.SummonerName.ToString(),
                        champion.Name,
                        champion.ChampionImage.FullFileName,
                        p.ChampLevel,
                        p.Kills,
                        p.Deaths,
                        p.Assists,
                        (p.Kills + p.Assists) / p.Deaths,
                        p.TotalMinionsKilled,
                        p.TotalMinionsKilled / gameDuration.Minutes,
                        p.Item0,
                        p.Item1,
                        p.Item2,
                        p.Item3,
                        p.Item4,
                        p.Item5,
                        p.Item6,
                        p.SummonerId == summonerId);
                }),
                g.Select(p => p.Side).Distinct().Single(),
                g.Select(p => p.Win).Distinct().Single()));
    }
} 