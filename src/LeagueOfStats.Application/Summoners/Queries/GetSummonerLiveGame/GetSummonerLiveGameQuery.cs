using Camille.RiotGames.SpectatorV4;
using LeagueOfStats.Application.ApiClients.RiotClient;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using MediatR;
using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerLiveGame;

public record GetSummonerLiveGameQuery(
        Guid Id)
    : IRequest<Result<LiveGameDto>>;

public class GetSummonerLiveGameQueryHandler
    : IRequestHandler<GetSummonerLiveGameQuery, Result<LiveGameDto>>
{
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IRiotClient _riotClient;
    private readonly IChampionRepository _championRepository;

    public GetSummonerLiveGameQueryHandler(
        ISummonerDomainService summonerDomainService,
        IRiotClient riotClient,
        IChampionRepository championRepository)
    {
        _summonerDomainService = summonerDomainService;
        _riotClient = riotClient;
        _championRepository = championRepository;
    }

    public Task<Result<LiveGameDto>> Handle(
        GetSummonerLiveGameQuery query,
        CancellationToken cancellationToken) =>
        _summonerDomainService.GetByIdAsync(query.Id)
            .Bind(summoner => _riotClient.GetSummonerLiveGameAsync(new GetSummonerLiveGameDto(
                    summoner.SummonerName,
                    summoner.SummonerId,
                    summoner.Region)))
            .Map(MapToSummonerLiveGameDtoAsync);
    
    private async Task<LiveGameDto> MapToSummonerLiveGameDtoAsync(CurrentGameInfo currentGameInfo)
    {
        var champions = (await _championRepository.GetAllAsync()).ToList();

        return new LiveGameDto(
            currentGameInfo.BannedChampions.Select(bc =>
            {
                Champion bannedChampion = champions.Single(c => c.RiotChampionId == (int)bc.ChampionId);

                return new LiveGameBannedChampionDto(
                    bannedChampion.Id,
                    bannedChampion.Name,
                    bannedChampion.ChampionImage.SplashUrl,
                    bc.PickTurn,
                    bc.TeamId.ToSide());
            }),
            currentGameInfo.Participants.GroupBy(p => p.TeamId).Select(g =>
            {
                return new LiveGameTeamDto(
                    g.Select(p =>
                    {
                        Champion championPlayedByParticipant =
                            champions.Single(c => c.RiotChampionId == (int)p.ChampionId);

                        return new LiveGameTeamParticipantDto(
                            championPlayedByParticipant.Id,
                            championPlayedByParticipant.Name,
                            championPlayedByParticipant.ChampionImage.SplashUrl,
                            p.SummonerName,
                            p.Bot,
                            (int)p.ProfileIconId,
                            p.Spell1Id,
                            p.Spell2Id);
                    }),
                    g.Key.ToSide());
            }),
            Duration.FromSeconds(currentGameInfo.GameLength),
            currentGameInfo.GameMode.ToGameMode(),
            Instant.FromUnixTimeMilliseconds(currentGameInfo.GameStartTime),
            currentGameInfo.GameType.ToGameType(),
            currentGameInfo.MapId.ToMap());
    }
}