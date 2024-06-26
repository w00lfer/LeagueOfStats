using Camille.RiotGames.MatchV5;
using LeagueOfStats.Application.ApiClients.RiotClient;
using LeagueOfStats.Application.Common.Errors;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Application.Summoners.Enums;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Enums;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Matches;
using LeagueOfStats.Domain.Matches.Enums;
using LeagueOfStats.Domain.Matches.Participants.Dtos;
using LeagueOfStats.Domain.Matches.Teams.Dtos;
using LeagueOfStats.Domain.Summoners;
using LeagueOfStats.Domain.Summoners.Dtos;
using MediatR;
using NodaTime;
using Match = LeagueOfStats.Domain.Matches.Match;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerMatchHistory;

public record GetSummonerMatchHistorySummaryQuery(
    Guid SummonerId,
    Instant GameEndedAt,
    MatchHistoryQueueFilter MatchHistoryQueueFilter,
    int Limit)
    : IRequest<Result<IEnumerable<MatchHistorySummaryDto>>>;

public class GetSummonerMatchHistorySummaryQueryHandler
    : IRequestHandler<GetSummonerMatchHistorySummaryQuery, Result<IEnumerable<MatchHistorySummaryDto>>>
{
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly ISummonerRepository _summonerRepository;
    private readonly IRiotClient _riotClient;
    private readonly IMatchDomainService _matchDomainService;
    private readonly IChampionRepository _championRepository;

    public GetSummonerMatchHistorySummaryQueryHandler(
        ISummonerDomainService summonerDomainService,
        ISummonerRepository summonerRepository,
        IRiotClient riotClient,
        IMatchDomainService matchDomainService,
        IChampionRepository championRepository)
    {
        _summonerDomainService = summonerDomainService;
        _summonerRepository = summonerRepository;
        _riotClient = riotClient;
        _matchDomainService = matchDomainService;
        _championRepository = championRepository;
    }

    public Task<Result<IEnumerable<MatchHistorySummaryDto>>> Handle(
        GetSummonerMatchHistorySummaryQuery query,
        CancellationToken cancellationToken) =>
        
        _summonerDomainService.GetByIdAsync(query.SummonerId)
            .Bind(summoner => _riotClient.GetSummonerMatchHistorySummaryAsync(new GetSummonerMatchHistoryDto(
                    summoner.Region,
                    summoner.Puuid,
                    query.Limit,
                    query.GameEndedAt,
                    query.MatchHistoryQueueFilter))
                .Bind(matchesFromRiotApi => GetOrCreateSummonersForMatchesByPuuid(
                        matchesFromRiotApi,
                        summoner.Region)
                    .Bind(async summoners =>
                    {
                        var champions = (await _championRepository.GetAllAsync()).ToList();

                        var addMatchDtos = matchesFromRiotApi.Select(matchFromRiotApi =>
                        {
                            var summonersPuuidsInMatch = matchFromRiotApi.Info.Participants
                                .Select(p => p.Puuid);

                            return MapMatchFromRiotApiToAddMatchdto(
                                matchFromRiotApi,
                                summoners
                                    .Where(s => summonersPuuidsInMatch.Contains(s.Puuid))
                                    .ToList(),
                                champions);
                        });

                        return await _matchDomainService.AddMatchesAsync(addMatchDtos);
                    })))
            .Map(matches => MapToMatchHistorySummaryDtosAsync(matches, query.SummonerId));

    private async Task<Result<List<Summoner>>> GetOrCreateSummonersForMatchesByPuuid(
        IEnumerable<Camille.RiotGames.MatchV5.Match> matchesFromRiotApi,
        Region region)
    {
        var uniqueSummoners = matchesFromRiotApi
            .SelectMany(m => m.Info.Participants)
            .Select(p => new SummonerInfoDto(p.Puuid, p.SummonerName, p.RiotIdTagline))
            .Distinct()
            .ToList();

        var existingSummoners =
            await _summonerRepository.GetByPuuidsAsync(uniqueSummoners.Select(s => s.Puuid));

        var existingSummonersPuuids = existingSummoners
            .Select(s => s.Puuid)
            .ToList();

        return await CreateSummonersUsingDataFromRiotApiAsync(
                uniqueSummoners.Where(s => existingSummonersPuuids.Contains(s.Puuid) is false),
                region)
            .Bind(summoners => Result.Success(summoners.Concat(existingSummoners).ToList()));
    }

    private async Task<Result<IEnumerable<Summoner>>> CreateSummonersUsingDataFromRiotApiAsync(
        IEnumerable<SummonerInfoDto> summonerInfoDtos,
        Region region)
    {
        var champions = (await _championRepository.GetAllAsync()).ToList();
        
        Result<CreateSummonerDto>[] createCreateSummonerDtoResults =
            await Task.WhenAll(summonerInfoDtos.Select(s =>
                CreateCreateSummonerDtosUsingDataFromRiotApiAsync(s, champions, region)));

        if (createCreateSummonerDtoResults.Any(r => r.IsFailure))
        {
            return new ApplicationError(string.Join(
                Environment.NewLine,
                createCreateSummonerDtoResults.Where(r => r.IsFailure).Select(r => r.AggregatedErrorMessages)));
        }

        var summoners = await _summonerDomainService.CreateMultipleAsync(createCreateSummonerDtoResults.Select(r => r.Value));

        return Result.Success(summoners);
    }

    private Task<Result<CreateSummonerDto>> CreateCreateSummonerDtosUsingDataFromRiotApiAsync(
        SummonerInfoDto summonerInfoDto,
        List<Champion> champions,
        Region region) =>
        _riotClient.GetSummonerByPuuidAsync(summonerInfoDto.Puuid, region)
            .Bind(summonerFromRiotApi => _riotClient.GetSummonerChampionMasteryByPuuidAsync(
                    summonerFromRiotApi.Puuid,
                    region)
                .Map(async summonerChampionMasteriesFromRiotApi =>
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
                        summonerChampionMasteriesFromRiotApi.Select(cm =>
                            new UpdateChampionMasteryDto(
                                champions.Single(c => c.RiotChampionId == (int)cm.ChampionId),
                                cm.ChampionLevel,
                                cm.ChampionPoints,
                                cm.ChampionPointsSinceLastLevel,
                                cm.ChampionPointsUntilNextLevel,
                                cm.ChestGranted,
                                cm.LastPlayTime,
                                cm.TokensEarned)));

                    return createSummonerDto;
                }));


    private AddMatchDto MapMatchFromRiotApiToAddMatchdto(
        Camille.RiotGames.MatchV5.Match matchFromRiotApi,
        List<Summoner> participatedSummonersInMatch,
        List<Champion> champions)
    {
        var addParticipantDtos = matchFromRiotApi.Info.Participants
            .Select(participant => MapParticipantToAddParticipantDto(
                participant,
                champions.Single(c => c.RiotChampionId == (int)participant.ChampionId),
                participatedSummonersInMatch.Single(s => s.SummonerId == participant.SummonerId)));

        var addTeamDtos = matchFromRiotApi.Info.Teams.
            Select(team => MapTeamToAddTeamDto(team, champions));

        var addMatchDto = new AddMatchDto(
            matchFromRiotApi.Metadata.MatchId,
            matchFromRiotApi.Info.GameVersion,
            Duration.FromSeconds(matchFromRiotApi.Info.GameDuration),
            Instant.FromUnixTimeMilliseconds(matchFromRiotApi.Info.GameStartTimestamp),
            Instant.FromUnixTimeMilliseconds(matchFromRiotApi.Info.GameEndTimestamp!.Value),
            matchFromRiotApi.Info.GameMode.ToGameMode(),
            matchFromRiotApi.Info.GameType.ToGameType(),
            matchFromRiotApi.Info.MapId.ToMap(),
            matchFromRiotApi.Info.PlatformId,
            matchFromRiotApi.Info.QueueId.ToQueue(),
            matchFromRiotApi.Info.TournamentCode,
            addParticipantDtos,
            addTeamDtos);

        return addMatchDto;
    }
    
    private async Task<IEnumerable<MatchHistorySummaryDto>> MapToMatchHistorySummaryDtosAsync(
        IEnumerable<Match> matches,
        Guid summonerId)
    {
        var champions = (await _championRepository.GetAllAsync()).ToList();

        var summonersIds = matches.SelectMany(m => m.Participants.Select(p => p.SummonerId)).ToArray();
        var summonersParticipatedInMatches = await _summonerRepository.GetAllAsync(summonersIds);
        
        return matches.Select(m =>
        {
            var participantAsSummoner = m.Participants.Single(p => p.SummonerId == summonerId);
            var championPlayedBySummoner = champions.Single(c => c.Id == participantAsSummoner.ChampionId);
            
            return new MatchHistorySummaryDto(
                m.Id,
                new MatchHistorySummarySummonerDto(
                    championPlayedBySummoner.Id,
                    championPlayedBySummoner.Name,
                    championPlayedBySummoner.ChampionImage.SplashUrl,
                    participantAsSummoner.ChampLevel,
                    participantAsSummoner.Kills,
                    participantAsSummoner.Deaths,
                    participantAsSummoner.Assists,
                    (participantAsSummoner.Kills + participantAsSummoner.Assists) / participantAsSummoner.Deaths,
                    participantAsSummoner.TotalMinionsKilled,
                    participantAsSummoner.TotalMinionsKilled / m.GameDuration.Minutes,
                    participantAsSummoner.Item0,
                    participantAsSummoner.Item1,
                    participantAsSummoner.Item2,
                    participantAsSummoner.Item3,
                    participantAsSummoner.Item4,
                    participantAsSummoner.Item5,
                    participantAsSummoner.Item6),
                MapParticipantsToMatchHistorySummaryTeamDtos(
                    m.Participants,
                    champions,
                    summonersParticipatedInMatches,
                    m.GameMode),
                m.GameVersion,
                m.GameDuration,
                m.GameStartTimeStamp,
                m.GameEndTimestamp,
                m.GameMode,
                m.GameType,
                m.Map);
        });
    }

    private IEnumerable<MatchHistorySummaryTeamDto> MapParticipantsToMatchHistorySummaryTeamDtos(
        IEnumerable<Domain.Matches.Participants.Participant> participants,
        IEnumerable<Champion> champions,
        IEnumerable<Summoner> summoners,
        GameMode gameMode)
    {
        if (gameMode is GameMode.Arena)
        {
            return participants.GroupBy(p => p.PlayerSubteamId).Select(g =>
                new MatchHistorySummaryTeamDto(
                    g.Select(p =>
                    {
                        Summoner summoner = summoners.Single(s => s.Id == p.SummonerId);
                        Champion champion = champions.Single(c => c.Id == p.ChampionId);
                        return new MatchHistorySummaryTeamParticipantDto(
                            champion.Id,
                            summoner.Id,
                            summoner.SummonerName.ToString(),
                            champion.Name,
                            champion.ChampionImage.SplashUrl);
                    }),
                    g.Select(p => p.Side).Distinct().Single(),
                    g.Select(p => p.Win).Distinct().Single()));
        }

        return participants.GroupBy(p => p.Side).Select(g =>
            new MatchHistorySummaryTeamDto(
                g.Select(p =>
                {
                    Summoner summoner = summoners.Single(s => s.Id == p.SummonerId);
                    Champion champion = champions.Single(c => c.Id == p.ChampionId);
                    return new MatchHistorySummaryTeamParticipantDto(
                        champion.Id,
                        summoner.Id,
                        summoner.SummonerName.ToString(),
                        champion.Name,
                        champion.ChampionImage.SplashUrl);
                }),
                g.Select(p => p.Side).Distinct().Single(),
                g.Select(p => p.Win).Distinct().Single()));
    }

    private AddParticipantDto MapParticipantToAddParticipantDto(
        Participant participant,
        Champion champion,
        Summoner summoner) =>
        new(
            champion,
            summoner,
            new AddPerksDto(
                new AddPerkStatsDto(
                    participant.Perks.StatPerks.Defense,
                    participant.Perks.StatPerks.Flex,
                    participant.Perks.StatPerks.Offense),
                participant.Perks.Styles.Select(perkStyle => new AddPerkStyleDto(perkStyle.Selections.Select(
                        perkStyleSelection => new AddPerkStyleSelectionDto(perkStyleSelection.Perk,
                            perkStyleSelection.Var1,
                            perkStyleSelection.Var2,
                            perkStyleSelection.Var3)),
                    perkStyle.Description,
                    perkStyle.Style))),
            participant.Assists,
            participant.ChampLevel,
            participant.DamageDealtToBuildings,
            participant.DamageDealtToObjectives,
            participant.DamageDealtToTurrets,
            participant.DamageSelfMitigated,
            participant.Deaths,
            participant.DetectorWardsPlaced,
            participant.DoubleKills,
            participant.FirstBloodKill,
            participant.FirstTowerKill,
            participant.GameEndedInEarlySurrender,
            participant.GameEndedInSurrender,
            participant.GoldEarned,
            participant.GoldSpent,
            participant.Item0,
            participant.Item1,
            participant.Item2,
            participant.Item3,
            participant.Item4,
            participant.Item5,
            participant.Item6,
            participant.ItemsPurchased,
            participant.KillingSprees,
            participant.Kills,
            participant.LargestCriticalStrike,
            participant.LargestKillingSpree,
            participant.LargestMultiKill,
            participant.LongestTimeSpentLiving,
            participant.MagicDamageDealt,
            participant.MagicDamageDealtToChampions,
            participant.MagicDamageTaken,
            participant.NeutralMinionsKilled,
            participant.NexusKills,
            participant.ObjectivesStolen,
            participant.PentaKills,
            participant.PhysicalDamageDealt,
            participant.PhysicalDamageDealtToChampions,
            participant.PhysicalDamageTaken,
            participant.Placement,
            participant.PlayerAugment1,
            participant.PlayerAugment2,
            participant.PlayerAugment3,
            participant.PlayerAugment4,
            participant.PlayerSubteamId,
            participant.QuadraKills,
            participant.Spell1Casts,
            participant.Spell2Casts,
            participant.Spell3Casts,
            participant.Spell4Casts,
            participant.SubteamPlacement,
            participant.Summoner1Casts,
            participant.Summoner1Id,
            participant.Summoner2Casts,
            participant.Summoner2Id,
            participant.TeamEarlySurrendered,
            participant.TeamId.ToSide(),
            participant.TeamPosition,
            participant.TimeCCingOthers,
            participant.TimePlayed,
            participant.TotalDamageDealt,
            participant.TotalDamageDealtToChampions,
            participant.TotalDamageShieldedOnTeammates,
            participant.TotalDamageTaken,
            participant.TotalHeal,
            participant.TotalHealsOnTeammates,
            participant.TotalMinionsKilled,
            participant.TotalTimeCCDealt,
            participant.TotalTimeSpentDead,
            participant.TotalUnitsHealed,
            participant.TripleKills,
            participant.TrueDamageDealt,
            participant.TrueDamageDealtToChampions,
            participant.TrueDamageTaken,
            participant.TurretKills,
            participant.TurretsLost,
            participant.TurretTakedowns,
            participant.VisionScore,
            participant.VisionWardsBoughtInGame,
            participant.WardsKilled,
            participant.WardsPlaced,
            participant.Win);

    private AddTeamDto MapTeamToAddTeamDto(Team team, List<Champion> champions) =>
        new(
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
            team.Bans.Select(ban =>
                new AddBanDto(champions.SingleOrDefault(c => c.RiotChampionId == (int)ban.ChampionId), ban.PickTurn)),
            team.TeamId.ToSide(),
            team.Win);

    private record SummonerInfoDto(
        string Puuid,
        string GameName,
        string TagLine);
}