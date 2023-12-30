using Camille.RiotGames.SpectatorV4;
using LeagueOfStats.Application.Common.Validators;
using LeagueOfStats.Application.RiotClient;
using LeagueOfStats.Domain.Common.Rails.Results;
using LeagueOfStats.Domain.Summoners;
using MediatR;

namespace LeagueOfStats.Application.Summoners.Queries.GetSummonerLiveGame;

public record GetSummonerLiveGameQuery(
        Guid Id)
    : IRequest<Result<SummonerLiveGameDto>>;

public class GetSummonerLiveGameQueryHandler : IRequestHandler<GetSummonerLiveGameQuery, Result<SummonerLiveGameDto>>
{
    private readonly IValidator<GetSummonerLiveGameQuery> _getSummonerLiveGameQueryValidator;
    private readonly ISummonerDomainService _summonerDomainService;
    private readonly IRiotClient _riotClient;

    public GetSummonerLiveGameQueryHandler(
        IValidator<GetSummonerLiveGameQuery> getSummonerLiveGameQueryValidator,
        ISummonerDomainService summonerDomainService,
        IRiotClient riotClient)
    {
        _getSummonerLiveGameQueryValidator = getSummonerLiveGameQueryValidator;
        _summonerDomainService = summonerDomainService;
        _riotClient = riotClient;
    }

    public Task<Result<SummonerLiveGameDto>> Handle(GetSummonerLiveGameQuery query, CancellationToken cancellationToken) =>
        _getSummonerLiveGameQueryValidator.ValidateAsync(query)
            .Bind(() => _summonerDomainService.GetByIdAsync(query.Id))
            .Bind(summoner => _riotClient.GetSummonerLiveGame(new GetSummonerLiveGameDto(
                    summoner.SummonerName,
                    summoner.SummonerId,
                    summoner.Region)))
            .Map(MapToSummonerLiveGameDto);

    private SummonerLiveGameDto MapToSummonerLiveGameDto(CurrentGameInfo currentGameInfo) => 
        new(currentGameInfo.ToString());
}