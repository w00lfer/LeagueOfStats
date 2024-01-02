using Camille.RiotGames.SpectatorV4;
using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.Extensions;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Champions;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using MediatR;
using NodaTime;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerLiveGame;

public record GetSummonerLiveGameQuery(
        Guid Id)
    : IRequest<Result<LiveGameDto>>;

public class GetSummonerLiveGameQueryHandler : IRequestHandler<GetSummonerLiveGameQuery, Result<LiveGameDto>>
{
    private readonly IValidator<GetSummonerLiveGameQuery> _getSummonerLiveGameQueryValidator;
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IRiotClient _riotClient;
    private readonly IChampionRepository _championRepository;

    public GetSummonerLiveGameQueryHandler(
        IValidator<GetSummonerLiveGameQuery> getSummonerLiveGameQueryValidator,
        ISummonerDomainService summonerDomainService,
        IRiotClient riotClient,
        IChampionRepository championRepository)
    {
        _getSummonerLiveGameQueryValidator = getSummonerLiveGameQueryValidator;
        _summonerDomainService = summonerDomainService;
        _riotClient = riotClient;
        _championRepository = championRepository;
    }

    public Task<Result<LiveGameDto>> Handle(GetSummonerLiveGameQuery query, CancellationToken cancellationToken) =>
        _getSummonerLiveGameQueryValidator.ValidateAsync(query)
            .Bind(() => _summonerDomainService.GetByIdAsync(query.Id))
            .Bind(summoner => _riotClient.GetSummonerLiveGame(new GetSummonerLiveGameDto(
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
                    bannedChampion.ChampionImage.FullFileName,
                    bc.PickTurn,
                    bc.TeamId.ToSide());
            }),
            currentGameInfo.Participants.GroupBy(p => p.TeamId).Select(g =>
            {
                return new LiveGameTeamDto(
                    g.Select(p =>
                    {
                        Champion championPlayedByParticipant = champions.Single(c => c.RiotChampionId == (int)p.ChampionId);

                        return new LiveGameTeamParticipantDto(
                            championPlayedByParticipant.Id,
                            championPlayedByParticipant.Name,
                            championPlayedByParticipant.ChampionImage.FullFileName,
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